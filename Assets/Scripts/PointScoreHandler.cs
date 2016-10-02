using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
                }
                else if (obj.tag == "Hexagon")
                {
                    player.GetComponent<Player>().score += scoreDestroyHexagon;
                    player.GetComponent<Player>().points += pointsDestroyHexagon;
                }
                else if (obj.tag == "Player")
                {
                    player.GetComponent<Player>().score += scoreDestroyPlayer;
                    player.GetComponent<Player>().points += pointsDestroyPlayer;

                }
            }
        }
    }
}
