﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
                addPart(new AxialCoordinate { x = 1, y = 0 }, "Triangle");
            }
        }

        public void addPart(AxialCoordinate location, string part)
        {
            // NOTE: type = 1 if player part. type = 0 if hexagon part. type = -1 if triangle part
            // Add a part to both the PartData, and the PhotonNetwork using Instantiate

            GameObject partType;
            int type;

            if(part == "Hexagon")
            {
                partType = hexagonPart;
                type = 0;
            } else if(part == "Triangle")
            {
                partType = trianglePart;
                type = -1;
            } else if(part == "Player")
            {
                partType = hexagonPart;
                type = 1;
            } else
            {
                Debug.LogError("Unknown part added: " + part);
                return;
            }
                

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
            }

        }

        public void addPart(Vector3 location, string part)
        {
            // Do a search for the hexagon closest to location with at least one open slot
            AxialCoordinate newLocation = hexData.getLocation(location);
            newLocation = hexData.getEmptyNeighbors(newLocation)[0];

            addPart(newLocation, part);
        }

        public void removePart(AxialCoordinate location)
		{
			PhotonView destroyedObject = PhotonView.Get(hexData.getPart(location).Value.shape);
			hexData.removePart(location);
			destroyedObject.RPC ("PunFadeOut", PhotonTargets.All);

            //PhotonNetwork.Destroy(((Part)hexData.getPart(location)).shape);
        }

        private Vector3 axialToPixel(AxialCoordinate location)
        {
            // Convert the axial position of the location to a pixel location
            Vector3 playerPosition = hexData.getPart(playerLocation).Value.shape.transform.position;

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
            AxialCoordinate lowestNeighbor = new AxialCoordinate { x = int.MaxValue/3, y = int.MaxValue/3 };
            foreach (AxialCoordinate neighbor in neighbors)
                if (neighbor.x + neighbor.y < lowestNeighbor.x + lowestNeighbor.y)
                    lowestNeighbor = neighbor;

            //Step 3: Rotate to that neighbor
            //XXX: I couldn't think of a way to do this except to hard-code the different configurations
            AxialCoordinate neighborNormal = new AxialCoordinate { x = location.x - lowestNeighbor.x, y = location.y - lowestNeighbor.y };
            int rotation = 0;

            if (neighborNormal.x == -1 && neighborNormal.y == 0)
                rotation = 90;
            else if (neighborNormal.x == -1 && neighborNormal.y == 1)
                rotation = 30;
            else if (neighborNormal.x == 0 && neighborNormal.y == 1)
                rotation = 330;
            else if (neighborNormal.x == 1 && neighborNormal.y == -1)
                rotation = 210;
            else if (neighborNormal.x == 1 && neighborNormal.y == 0)
                rotation = 270;
            else
                rotation = 150;

            return Quaternion.Euler(0, 0, rotation);
        }
    }
}