using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class GameOverScreen : MonoBehaviour
    {
        public GameObject yourScoreTextObj;
        public GameObject[] highScoresTextObjs;

        public void Start()
        {
            yourScoreTextObj.GetComponent<Text>().text = "Your final score was " + PlayerPrefs.GetFloat("finalScore").ToString();
        }

        public void BackToMenu()
        {
            Application.LoadLevel(0);
        }
    }
}