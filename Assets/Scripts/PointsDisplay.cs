using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TeamBronze.HexWars
{
    public class PointsDisplay : MonoBehaviour
    {

        Text points;
        GameObject player;

        // Use this for initialization
        void Start()
        {
            points = GetComponent<Text>();
            player = GameObject.FindGameObjectWithTag("LocalPlayer");
        }

        // Update is called once per frame
        void Update()
        {

            if (!player) player = GameObject.FindGameObjectWithTag("LocalPlayer");

            points.text = "Points/Score: " + player.GetComponent<Player>().points + "/" + player.GetComponent<Player>().rb.mass.ToString("0.0");

        }
    }

}