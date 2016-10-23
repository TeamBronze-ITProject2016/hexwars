/* Scoreboard.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Displays a scoreboard with the current score of all in-game players.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using TeamBronze.HexWars;
#if UNITY_5_3
using UnityEngine.Experimental.Networking;
#else
using UnityEngine.Networking;
#endif

namespace TeamBronze.HexWars
{
    public class Scoreboard : MonoBehaviour
    {
        [Tooltip("Amount to multiply player mass by (score is a multiple of player mass)")]
        public float scoreMultiplier = 10.0f;

        [Tooltip("How frequently to update scoreboard in seconds (may be too CPU intensive to do every frame")]
        public float updateInterval = 1.0f;

        [Tooltip("The maximum number of scores to show on the scoreboard (highest scores will be shown")]
        public int maxScores = 4;

        private GameObject scoresDisplay;
        private float localPlayerHighestScore;
        private float t;

        // Initialize
        void Start()
        {
            scoresDisplay = transform.FindChild("Scores").gameObject;
            t = updateInterval;
        }

        void Update()
        {
            UpdateLocalPlayerHighestScore();

            // Only update scoreboard every updateInterval
            if (t > 0.0f)
            {
                t -= Time.deltaTime;
                return;
            }
            else
                t = updateInterval;

            ClearScores();

            // Find all players including local player
            GameObject[] otherPlayers = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] players = new GameObject[otherPlayers.Length + 1];
            players[0] = GameObject.FindGameObjectWithTag("LocalPlayer");
            float[] playerScores = new float[players.Length];

            Array.Copy(otherPlayers, 0, players, 1, otherPlayers.Length);

            // Find all player scores
            for (int i = 0; i < playerScores.Length; i++)
            {
                playerScores[i] = players[i].GetComponent<Rigidbody2D>().mass * scoreMultiplier;
            }

            // Sort playerScores and players based on playerScores
            Array.Sort(playerScores, players); 
            Array.Reverse(playerScores);
            Array.Reverse(players);

            // Number of scores to display, either maxScores or less if there aren't enough players
            int scoresToDisplay;

            if (players.Length < maxScores)
                scoresToDisplay = players.Length;
            else
                scoresToDisplay = maxScores;

            // Display each score
            for(int i = 0; i < scoresToDisplay; i ++)
            {
                string playerName = players[i].GetPhotonView().owner.name;
                string playerScore = playerScores[i].ToString();
                playerScore = RemoveDecimals(playerScore);
                Text text = AddText(" " + (i + 1) + ". " + playerName + " - " + playerScore);
                text.color = Color.Lerp(players[i].GetComponent<SpriteRenderer>().color, Color.black, 0.5f);
                text.fontStyle = FontStyle.Bold;
            }
        }

        // Returns the highest score that the local player has had so far in this game
        public float GetLocalPlayerHighestScore()
        {
            return localPlayerHighestScore;
        }

        // Finds the local player's current score, and updates their highest score if their current score is greater
        private void UpdateLocalPlayerHighestScore()
        {
            GameObject localPlayerObj = GameObject.FindGameObjectWithTag("LocalPlayer");

            if (localPlayerObj == null)
                return;

            float cur = localPlayerObj.GetComponent<Rigidbody2D>().mass * scoreMultiplier;

            if (cur > localPlayerHighestScore)
                localPlayerHighestScore = cur;
        }

        // Add text to the scoreboard
        private Text AddText(string text)
        {
            GameObject textGO = new GameObject("myTextGO");
            textGO.transform.SetParent(scoresDisplay.transform);
            Text myText = textGO.AddComponent<Text>();
            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            myText.font = ArialFont;
            myText.color = Color.black;
            myText.alignment = TextAnchor.MiddleLeft;
            myText.text = text;

            return myText;
        }

        // Clears the scoreboard
        private void ClearScores()
        {
            for(int i = 0; i < scoresDisplay.transform.childCount; i++)
            {
                Destroy(scoresDisplay.transform.GetChild(i).gameObject);
            }
        }

        // Truncates string at the last '.' character (inclusive)
        private string RemoveDecimals(string s)
        {
            if (s.IndexOf(".") < 0)
                return s;

            return s.Substring(0, s.IndexOf("."));
        }
    }
}
