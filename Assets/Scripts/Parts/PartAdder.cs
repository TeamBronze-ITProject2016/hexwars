/* PartAdder.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Manages all adding and removing parts from players
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TeamBronze.HexWars
{
    public class PartAdder : Photon.MonoBehaviour
    {
        // Stores the prefabs for each gameobject part
        public GameObject hexagonPart;
        public GameObject trianglePart;

        // Distance of hexagon parts from each other
        public float size = 1.23f;

        public PartData hexData = new PartData();
        public int maxNeighbors = 4;

        private Part player;
        private AxialCoordinate playerLocation = new AxialCoordinate { x = 0, y = 0 };

        void Start()
        {
            InvokeRepeating("connectPartAdder", 0.2f, 1.0f);
        }
        
        void connectPartAdder()
        {
            if (player.shape == null)
            {
                player = new Part { shape = GameObject.FindGameObjectWithTag("LocalPlayer"), type = 1 };
                hexData.addPart(playerLocation, player);
                addPart(new AxialCoordinate { x = -1, y = 0 }, "Triangle");
            }
        }
        
        public void addPart(AxialCoordinate location, string part)
        {
            // NOTE: type = 1 if player part. type = 0 if hexagon part. type = -1 if triangle part
            // Add a part to both the PartData, and the PhotonNetwork using Instantiate

            GameObject partType;
            int type;
            
            if (part == "Hexagon")
            {
                partType = hexagonPart;
                type = 0;
            }
            else if (part == "Triangle")
            {
                partType = trianglePart;
                type = -1;
            }
            else if (part == "Player")
            {
                partType = hexagonPart;
                type = 1;
            }
            else
            {
                Debug.LogError("Unknown part added: " + part);
                return;
            }

            // Freeze the player until the part has been added
            Vector3 playerLocation = player.shape.transform.position;
            Quaternion playerRotation = player.shape.transform.rotation;

            player.shape.transform.position = Vector3.zero;
            player.shape.transform.rotation = Quaternion.Euler(Vector3.zero);

            if (hexData.checkPart(location))
            {
                // Only add axial rotation if the part is a triangle
                Quaternion rotation = player.shape.transform.rotation;
                if (type == -1)
                    rotation *= axialToRotation(location);

                // Instantiate a new part
                GameObject newPart = PhotonNetwork.Instantiate(partType.name, axialToPixel(location), rotation, 0);

                // Add the part as a child of the player hexagon
                newPart.transform.parent = player.shape.transform;
                Part addedPart = new Part { shape = newPart, type = type };
                hexData.addPart(location, addedPart);

                /*Notify replay manager that part has been added*/
                EventManager.pushEventDataFloat("partadded", newPart.transform.position.z);
                EventManager.pushEventDataFloat("partadded", newPart.transform.position.y);
                EventManager.pushEventDataFloat("partadded", newPart.transform.position.x);
                EventManager.triggerEvent("partadded");
            }

            // Return the player to their position before the part was added
            player.shape.transform.position = playerLocation;
            player.shape.transform.rotation = playerRotation;

            /*// Update the server when part added
            PointScoreHandler pointsScoreHandler = player.shape.GetComponent<PointScoreHandler>();
            pointsScoreHandler.updateServerScore();

            // Update the scoreboard
            GameObject scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard");
            PhotonView scoreboardView = PhotonView.Get(scoreboard);
            scoreboardView.RPC("UpdateScoresBoard", PhotonTargets.All);*/
        }
        
        public bool addRandomPart(string part="None")
        {
            int maxCount = hexData.dataTable.Count;
            int counter = 0;
            // Get a random location
            foreach (AxialCoordinate location in RandomKeys(hexData.dataTable))
            {
                if (counter > maxCount) return false; counter++;
                if (hexData.getPart(location) == null) continue;
                if (hexData.getPart(location).Value.type == -1) continue;
                List<AxialCoordinate> randLocations = hexData.getEmptyNeighbors(location);
                System.Random rnd = new System.Random();

                if (part == "None")
                {
                    part = "Triangle";
                    if (Random.Range(0, 2) == 1)
                        part = "Hexagon";
                }

                // Ensure that adding this part doesn't exceed the max allowed per hexagon
                int localMaxNeighbors;
                localMaxNeighbors = maxNeighbors;
                if (part == "Triangle") localMaxNeighbors -= 1;
                if (hexData.getFullNeighbors(location).Count >= localMaxNeighbors) {continue;}

                int size = randLocations.Count;
                AxialCoordinate randomSpot = randLocations[rnd.Next(size)];
                addPart(randomSpot, part);
                return true;
            }

            return false;

        }
        
        // From http://stackoverflow.com/questions/1028136/random-entry-from-dictionary
        public IEnumerable<TKey> RandomKeys<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            System.Random rand = new System.Random();
            List<TKey> keys = Enumerable.ToList(dict.Keys);
            int size = dict.Count;
            while (true) yield return keys[rand.Next(size)];
        }
        
        public void removePart(AxialCoordinate location)
        {
            try
            {
                PhotonView destroyedObject = PhotonView.Get(((Part)hexData.getPart(location)).shape);
                PhotonNetwork.Destroy(destroyedObject);
            }
            catch
            {
                Debug.Log("Could not destroy Photon GameObject");
            }

            hexData.removePart(location);
        }
        
        private Vector3 axialToPixel(AxialCoordinate location)
        {
            // Convert the axial position of the location to a pixel location

            float x; float y;
            x = size * Mathf.Sqrt(3f) * (location.x + location.y / 2f);
            y = size * (3f / 2f) * location.y;
            return new Vector3(x + player.shape.transform.position.x, y + player.shape.transform.position.y, 0);
        }
        
        private Quaternion axialToRotation(AxialCoordinate location)
        {
            // Convert an axial location to the rotation that that GameObject should be at
            // Step 1: Find all neighboring GameObjects
            List<AxialCoordinate> neighbors = hexData.getFullHexNeighbors(location);

            // Step 2: Pick the neighbor with the x+y value closest to 0 as the one to rotate to. This ensures that the part is always facing outwards
            AxialCoordinate lowestNeighbor = new AxialCoordinate { x = int.MaxValue / 3, y = int.MaxValue / 3 };
            foreach (AxialCoordinate neighbor in neighbors)
                if (neighbor.x + neighbor.y < lowestNeighbor.x + lowestNeighbor.y)
                    lowestNeighbor = neighbor;

            //Step 3: Rotate to that neighbor
            AxialCoordinate neighborNormal = new AxialCoordinate { x = location.x - lowestNeighbor.x, y = location.y - lowestNeighbor.y };
            int rotation = hexData.getRotationFromNeighbor(neighborNormal);

            return Quaternion.Euler(0, 0, rotation);
        }
    }
}