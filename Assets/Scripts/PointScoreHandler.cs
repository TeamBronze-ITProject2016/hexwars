using UnityEngine;
using System.Collections.Generic;
using System.Net;

namespace TeamBronze.HexWars
{
    public class PointScoreHandler : MonoBehaviour
    {
        // Points gained when destorying specific objects
        public int pointsDestoryTriangle;
        public int pointsDestroyHexagon;
        public int pointsDestroyPlayer;
        // Score gained when destroying specific objects
        public int scoreDestoryTriangle;
        public int scoreDestroyHexagon;
        public int scoreDestroyPlayer;
        
        // Update player score and points after destorying object
        public void update(List<AxialCoordinate> listToDestroy)
        {
            Player player = gameObject.GetComponent<Player>();
            PartAdder partAdder = GameObject.FindGameObjectWithTag("PartAdder").GetComponent<PartAdder>();
            // Score for each object destoryed
            foreach (AxialCoordinate coord in listToDestroy)
            {
                GameObject obj = partAdder.hexData.getPart(coord).Value.shape;
                if (obj.tag == "Triangle")
                {
                    player.GetComponent<Player>().score += scoreDestoryTriangle;
                    player.GetComponent<Player>().points += pointsDestoryTriangle;
                    updateServerScore(player);
                }
                else if (obj.tag == "Hexagon")
                {
                    player.GetComponent<Player>().score += scoreDestroyHexagon;
                    player.GetComponent<Player>().points += pointsDestroyHexagon;
                    updateServerScore(player);
                }
                else if (obj.tag == "Player")
                {
                    player.GetComponent<Player>().score += scoreDestroyPlayer;
                    player.GetComponent<Player>().points += pointsDestroyPlayer;
                    updateServerScore(player);
                }
            }
        }

        // Updates server with current score
        public void updateServerScore(Player player)
        {
            string url = "http://128.199.229.64/hexwars/";
            string name = PhotonNetwork.player.name + "/";
            string score = player.score.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + name + score);
        }
    }
}
