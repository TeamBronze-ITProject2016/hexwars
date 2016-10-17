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
            string highestScoreStr = PlayerPrefs.GetFloat("highestScore").ToString();
            highestScoreStr = highestScoreStr.Substring(0, highestScoreStr.LastIndexOf('.'));
            yourScoreTextObj.GetComponent<Text>().text = "Your highest score was " + highestScoreStr;

            GetHighScoresFromServer();
        }

        public void BackToMenu()
        {
            Application.LoadLevel(0);
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

            Debug.Log(responseString);

            string[] names = new string[highScoresTextObjs.Length];
            string[] scores = new string[highScoresTextObjs.Length];

            for(int i = 0; i < highScoresTextObjs.Length; i++)
            {
                // Make sure not empty
                if (responseString.IndexOf(",") < 0)
                    break;

                // Get name
                names[i] = responseString.Substring(0, responseString.IndexOf(","));
                responseString = responseString.Substring(responseString.IndexOf(",") + 1);

                Debug.Log(responseString);

                // Get score and trim string
                scores[i] = responseString.Substring(0, responseString.IndexOf(","));
                scores[i] = scores[i].Substring(0, scores[i].IndexOf("."));

                // Make sure not last
                if (responseString.IndexOf(",") < 0)
                    break;

                // Trim string
                responseString = responseString.Substring(responseString.IndexOf(",") + 1);
            }
            
            for(int i = 0; i < highScoresTextObjs.Length; i++)
            {
                highScoresTextObjs[i].GetComponent<Text>().text = (i + 1) + ". " + names[i] + " - " + scores[i];
            }
        }
    }
}