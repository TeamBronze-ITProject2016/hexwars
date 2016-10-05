using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.Networking;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TeamBronze.HexWars
{
    public class PointScoreHandler : Photon.MonoBehaviour
    {
        // Points gained when destorying specific objects
        public static int pointsDestoryTriangle = 1;
        public static int pointsDestroyHexagon = 2;
        public static int pointsDestroyPlayer = 3;
        // Score gained when destroying specific objects
        public static int scoreDestoryTriangle = 1;
        public static int scoreDestroyHexagon = 2;
        public static int scoreDestroyPlayer = 3;

        [PunRPC]
        public void updateDestructableServer()
        {
            Player player = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
            player.score++;

            updateServerScore();
        }

        [PunRPC]
        // Update player score and points after destorying object
        public void updateListServer(string stringDataToDestroy)
        {
            Player localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
            PartAdder partAdder = GameObject.FindGameObjectWithTag("PartAdder").GetComponent<PartAdder>();

            var ins = new MemoryStream(Convert.FromBase64String(stringDataToDestroy));
            var bf = new BinaryFormatter();
            List<AxialCoordinate> listToDestroy = (List<AxialCoordinate>)bf.Deserialize(ins);

            Debug.Log("WTF");

            // Score for each object destoryed
            foreach (AxialCoordinate coord in listToDestroy)
            {
                Debug.Log(coord.x + "," + coord.y);

                GameObject obj = partAdder.hexData.getPart(coord).Value.shape;
                if (obj.tag == "Triangle")
                {
                    localPlayer.GetComponent<Player>().score += scoreDestoryTriangle;
                    localPlayer.GetComponent<Player>().points += pointsDestoryTriangle;
                    updateServerScore();
                }
                else if (obj.tag == "Hexagon")
                {
                    localPlayer.GetComponent<Player>().score += scoreDestroyHexagon;
                    localPlayer.GetComponent<Player>().points += pointsDestroyHexagon;
                    updateServerScore();
                }
                else if (obj.tag == "Player")
                {
                    localPlayer.GetComponent<Player>().score += scoreDestroyPlayer;
                    localPlayer.GetComponent<Player>().points += pointsDestroyPlayer;
                    updateServerScore();
                }
            }
        }

        // Updates server with current score
        [PunRPC]
        public void updateServerScore()
        {
            Player localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
            string url = "http://128.199.229.64/hexwars/";
            string name = PhotonNetwork.player.ID + "/";
            string score = localPlayer.score.ToString();

            UnityWebRequest request = UnityWebRequest.Get(url + name + score);
            request.Send();

            Debug.Log ("update");
        }
    }
}
