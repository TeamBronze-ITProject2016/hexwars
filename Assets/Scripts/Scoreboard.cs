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
        public float scoreMultiplier = 10.0f;
        public float updateInterval = 1.0f;
        public int maxScores = 4;

        private GameObject scoresDisplay;
        private float localPlayerHighestScore;
        private float t;

        // Use this for initialization
        void Start()
        {
            scoresDisplay = transform.FindChild("Scores").gameObject;
            t = updateInterval;

            // ---------- Old stuff ----------
            //UpdateScoresBoard();
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

            GameObject[] otherPlayers = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] players = new GameObject[otherPlayers.Length + 1];
            players[0] = GameObject.FindGameObjectWithTag("LocalPlayer");
            float[] playerScores = new float[players.Length];

            Array.Copy(otherPlayers, 0, players, 1, otherPlayers.Length);

            for (int i = 0; i < playerScores.Length; i++)
            {
                playerScores[i] = players[i].GetComponent<Rigidbody2D>().mass * scoreMultiplier;
            }

            Array.Sort(playerScores, players); // sort playerScores and players based on playerScores
            Array.Reverse(playerScores);
            Array.Reverse(players);

            int scoresToDisplay;

            if (players.Length < maxScores)
                scoresToDisplay = players.Length;
            else
                scoresToDisplay = maxScores;

            for(int i = 0; i < scoresToDisplay; i ++)
            {
                string playerName = players[i].GetPhotonView().owner.name;
                string playerScore = playerScores[i].ToString();
                playerScore = playerScore.Substring(0, playerScore.IndexOf("."));
                Text text = AddText(" " + (i + 1) + ". " + playerName + " - " + playerScore);
                text.color = players[i].GetComponent<SpriteRenderer>().color;
                text.fontStyle = FontStyle.Bold;
            }

            // ---------- Old stuff ----------
            /*GameObject scoresDisplay = transform.FindChild("PlayerBar").transform.FindChild("PlayerPointScore").gameObject;
            GameObject player = GameObject.FindGameObjectWithTag("LocalPlayer");
            Text text = scoresDisplay.GetComponent<Text>();
            text.text = "Points/Score: " + player.GetComponent<Player>().points + "/" + player.GetComponent<Player>().rb.mass.ToString();*/
        }

        void UpdateLocalPlayerHighestScore()
        {
            float cur = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Rigidbody2D>().mass * scoreMultiplier;

            if (cur > localPlayerHighestScore)
                localPlayerHighestScore = cur;
        }

        public float GetLocalPlayerHighestScore()
        {
            return localPlayerHighestScore;
        }

        Text AddText(string text)
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

        void ClearScores()
        {
            for(int i = 0; i < scoresDisplay.transform.childCount; i++)
            {
                Destroy(scoresDisplay.transform.GetChild(i).gameObject);
            }
        }

        /*
        [PunRPC]
        IEnumerator UpdateScoresBoard () {
            // Remove previous scores
            GameObject scoresDisplay = transform.FindChild("Scores").gameObject;

            string url = "http://128.199.229.64/hexwars";
            WWW www = new WWW(url);
            yield return www;
            // Format string into array
            // http://stackoverflow.com/questions/19178983/how-in-c-sharp-to-convert-a-string-of-comma-separated-bracket-enclosed-nested
            string s = www.text;
            var result = s
                   .Split(']')
                   .Select(i => i.Replace('[', ' '))
                   .Select(i => i.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .ToList()).ToList();

            for(int i = 1; i <= maxScores; i++)
            {
                string playerscore;
                try
                {
                    playerscore = i + ". " + result[i][0].Trim('"').Remove(0,3) + ":" + result[i][1];
                    Debug.Log(result[i][0].Trim('"').Remove(0, 3));
                    try
                    {
                        scoresDisplay.transform.GetChild(i).gameObject.GetComponent<Text>().text = playerscore;
                    }
                    catch
                    {
                        AddText(playerscore);
                    }
                }
                catch
                {
                    Destroy(scoresDisplay.transform.GetChild(i).gameObject);
                }
            }
        }
        */
    }
}
