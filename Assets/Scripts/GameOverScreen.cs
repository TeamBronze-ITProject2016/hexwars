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
        public GameObject yourScoreTextObj;
        public GameObject[] highScoresTextObjs;

        public void Start()
        {
            float highestScore = PlayerPrefs.GetFloat("highestScore");
            string highestScoreStr = highestScore.ToString();
            highestScoreStr = highestScoreStr.Substring(0, highestScoreStr.LastIndexOf('.'));
            yourScoreTextObj.GetComponent<Text>().text = "Your highest score was " + highestScoreStr + ".";
            SendScoreToServer(highestScore);
            GetHighScoresFromServer();
        }

        public void BackToMenu()
        {
            Application.LoadLevel(0);
        }

        void SendScoreToServer(float score)
        {
            string playerName = PlayerPrefs.GetString("PlayerName");

            var request = (HttpWebRequest)WebRequest.Create("http://128.199.229.64/hexwars/" + playerName + "/" + score.ToString());
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Debug.Log(responseString);
        }

        public void GetHighScoresFromServer()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://128.199.229.64/hexwars");
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

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
            
            for(int i = 0; i < highScoresTextObjs.Length; i++)
            {
                highScoresTextObjs[i].GetComponent<Text>().text = (i + 1) + ". " + names[i] + " - " + scores[i];
            }
        }

        private string RemoveDecimals(string s)
        {
            if (s.IndexOf(".") < 0)
                return s;

            return s.Substring(0, s.IndexOf("."));
        }
    }
}