/* GameOverScreen.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Manages game over screen scores and GUI components
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System;

namespace TeamBronze.HexWars
{
    public class GameOverScreen : MonoBehaviour
    {
        // Game object with text showing your highest score for the previous game
        public GameObject yourScoreTextObj;

        // Game objects with text showing the all-time high scores
        public GameObject[] highScoresTextObjs;

        public void Start()
        {
            // Display player's highest score from the previous game
            float highestScore = PlayerPrefs.GetFloat("highestScore");
            string highestScoreStr = highestScore.ToString();
            highestScoreStr = RemoveDecimals(highestScoreStr);
            yourScoreTextObj.GetComponent<Text>().text = "Your highest score was " + highestScoreStr + ".";

            // Send player's score to the server
            SendScoreToServer(highestScore);

            // Get all-time high scores from the server
            GetHighScoresFromServer();
        }

        // Return to main menu
        public void BackToMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        // Send a score to the server
        private void SendScoreToServer(float score)
        {
            string playerName = PlayerPrefs.GetString("PlayerName");

            var request = (HttpWebRequest)WebRequest.Create("http://128.199.229.64/hexwars/" + playerName + "/" + score.ToString());
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Debug.Log(responseString);
        }

        // Get high scores from the server
        public void GetHighScoresFromServer()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://128.199.229.64/hexwars");
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Remove characters to make string easier to parse
            responseString = responseString.Replace("\"", "");
            responseString = responseString.Replace("[", "");
            responseString = responseString.Replace("]", "");
            responseString = responseString.Replace(" ", "");
            responseString += ",";

            string[] names = new string[highScoresTextObjs.Length];
            string[] scores = new string[highScoresTextObjs.Length];

            for(int i = 0; i < highScoresTextObjs.Length; i++)
            {
                // Make sure not empty
                if (responseString == "")
                    break;

                // Get name
                names[i] = responseString.Substring(0, responseString.IndexOf(","));
                responseString = responseString.Substring(responseString.IndexOf(",") + 1);

                // Get score and trim string
                scores[i] = responseString.Substring(0, responseString.IndexOf(","));
                scores[i] = RemoveDecimals(scores[i]);

                // Trim string
                responseString = responseString.Substring(responseString.IndexOf(",") + 1);
            }
            
            // Update score text objs with high scores
            for(int i = 0; i < highScoresTextObjs.Length; i++)
            {
                highScoresTextObjs[i].GetComponent<Text>().text = (i + 1) + ". " + names[i] + " - " + scores[i];
            }
        }

        // Trim everything after-and-including the last '.' in a string
        private string RemoveDecimals(string s)
        {
            if (s.IndexOf(".") < 0)
                return s;

            return s.Substring(0, s.IndexOf("."));
        }
    }
}