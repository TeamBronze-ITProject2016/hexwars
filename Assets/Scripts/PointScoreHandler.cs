using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_5_3
using UnityEngine.Experimental.Networking;
#else
using UnityEngine.Networking;
#endif

namespace TeamBronze.HexWars
{
    public class PointScoreHandler : Photon.MonoBehaviour
    {
        // Points gained when destorying specific objects
        public int pointsDestoryTriangle = 1;
        public int pointsDestroyHexagon = 2;
        public int pointsDestroyPlayer = 3;
        public int pointsDestroyDestructable = 1;
        public int pointsDestroyEnemy = 4;

        // Add score after destroying a destructable
        [PunRPC]
        public void updateLocalPointsDestructable()
        {
            Player player = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
            player.points += pointsDestroyDestructable;
        }

        // Add score after destroying a destructable
        [PunRPC]
        public void updateLocalPointsEnemy()
        {
            Player player = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
            player.points += pointsDestroyEnemy;
        }

        [PunRPC]
        // Update player score and points after destorying object
        public void updateLocalPointsList(string stringDataToDestroy)
        {
            Player localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
            PartAdder partAdder = GameObject.FindGameObjectWithTag("PartAdder").GetComponent<PartAdder>();

            // Convert from string data stream back to list
            var ins = new MemoryStream(Convert.FromBase64String(stringDataToDestroy));
            var bf = new BinaryFormatter();
            List<AxialCoordinate> listToDestroy = (List<AxialCoordinate>)bf.Deserialize(ins);

            // Score for each object destoryed
            foreach (AxialCoordinate coord in listToDestroy)
            {
                GameObject obj = partAdder.hexData.getPart(coord).Value.shape;
                // Update score and points according to what object was destroyed
                if (obj.tag == "Triangle")
                {
                    localPlayer.GetComponent<Player>().points += pointsDestoryTriangle;
                }
                else if (obj.tag == "Hexagon")
                {
                    localPlayer.GetComponent<Player>().points += pointsDestroyHexagon;
                }
                else if (obj.tag == "Player")
                {
                    localPlayer.GetComponent<Player>().points += pointsDestroyPlayer;
                }
            }
        }


        // Updates server with current score
        [PunRPC]
        public void updateServerScore()
        {
            PartAdder partAdder = GameObject.FindGameObjectWithTag("PartAdder").GetComponent<PartAdder>();
            Player localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
            string url = "http://128.199.229.64/hexwars/";
            string name = PhotonNetwork.player.ID + "/";
            string score = localPlayer.rb.mass.ToString();

            UnityWebRequest request = UnityWebRequest.Get(url + name + score);
            request.Send();
        }
    }
}
