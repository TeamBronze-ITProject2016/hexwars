using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PointsText : MonoBehaviour
    {
        private GameObject localPlayer;
        private Text text;

        void Start()
        {
            text = GetComponent<Text>();
        }

        void Update()
        {
            if(localPlayer == null)
            {
                localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
                return;
            }

            text.text = localPlayer.GetComponent<Player>().points.ToString();
        }
    }
}