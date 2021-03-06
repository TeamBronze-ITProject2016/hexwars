﻿/* PartData.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Stores data about where/which parts are added to the local player
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System;

    public struct Part
    {
        public GameObject shape;
        public int type;
    }

    [Serializable]
    #pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    #pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct AxialCoordinate
    #pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    #pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public int x;
        public int y;

        public static bool operator ==(AxialCoordinate a1, AxialCoordinate a2)
        {
            return (a1.x == a2.x && a1.y == a2.y);
        }

        public static bool operator !=(AxialCoordinate a1, AxialCoordinate a2)
        {
            return !(a1.x == a2.x && a1.y == a2.y);
        }

        public static AxialCoordinate operator +(AxialCoordinate a1, AxialCoordinate a2)
        {
            return new AxialCoordinate { x = a1.x + a2.x, y = a1.y + a2.y };
        }
    }

    public class PartData
    {
        // Stores children gameObjects
        public Dictionary<AxialCoordinate, Part?> dataTable = new Dictionary<AxialCoordinate, Part?>();

        private List<AxialCoordinate> directions = new List<AxialCoordinate>();

        private AxialCoordinate player = new AxialCoordinate { x = 0, y = 0 };

        public PartData()
        {
            directions.Add(new AxialCoordinate { x = 0, y = -1 });
            directions.Add(new AxialCoordinate { x = 1, y = -1 });
            directions.Add(new AxialCoordinate { x = 1, y = 0 });
            directions.Add(new AxialCoordinate { x = 0, y = 1 });
            directions.Add(new AxialCoordinate { x = -1, y = 1 });
            directions.Add(new AxialCoordinate { x = -1, y = 0 });
        }

        public bool checkPart(AxialCoordinate location)
        {
            // Check that a part can be added to the location provided. Will return true if:
            // 1. location has no parts already attached
            // 2. a path exists from location to player
            if (pathExistsToPlayer(location) && getPart(location) == null)
                return true;
            else return false;
        }

        public bool addPart(AxialCoordinate location, Part? part)
        {
            // Returns true if the part was successfully added
            if (checkPart(location))
                dataTable[location] = part;
            else return false;
            return true;
        }

        public Part? getPart(AxialCoordinate location)
        {
            Part? val = new Part?();
            dataTable.TryGetValue(location, out val);
            return val;
        }

        public List<AxialCoordinate> getEmptyNeighbors(AxialCoordinate location)
        {
            List<AxialCoordinate> empty = new List<AxialCoordinate>();
            if (getPart(location).Value.type == -1) return empty; // triangles don't have any empty neighbors
            foreach (AxialCoordinate direction in directions)
            {
                AxialCoordinate neighbor = location + direction;
                if (getPart(neighbor) == null) empty.Add(neighbor);
            }
            return empty;
        }

        public List<AxialCoordinate> getFullNeighbors(AxialCoordinate location)
        {
            List<AxialCoordinate> full = new List<AxialCoordinate>();
            if (getPart(location).Value.type == -1) { full.Add((AxialCoordinate)getNeighborFromTriangle(location)); return full; }
            foreach (AxialCoordinate direction in directions)
            {
                AxialCoordinate neighbor = location + direction;
                if (getPart(neighbor) != null) full.Add(neighbor);
            }

            return full;
        }

        public List<AxialCoordinate> getFullHexNeighbors(AxialCoordinate location)
        {
            List<AxialCoordinate> full = new List<AxialCoordinate>();

            if (getPart(location) != null && getPart(location).Value.type == -1)
            {
                // Location is a triangle, there's only one possible neighbor!
                AxialCoordinate? neighbor = getNeighborFromTriangle(location);
                if(neighbor != null) full.Add((AxialCoordinate)neighbor);
            }
            else
                foreach (AxialCoordinate direction in directions)
                {
                    AxialCoordinate neighbor = location + direction;
                    if (getPart(neighbor) != null && getPart(neighbor).Value.type != -1) full.Add(neighbor);
                }

            return full;
        }

        public void removePart(AxialCoordinate location)
        {
            // Remove a part
            dataTable[location] = null;
        }

        public AxialCoordinate getLocation(Vector3 location)
        {
            // Given a pixel coordinate, return the AxialCoordinate location of the closest part

            float minDistance = float.MaxValue;
            AxialCoordinate minCoordinate = player;

            foreach (KeyValuePair<AxialCoordinate, Part?> part in dataTable)
            {
                if (part.Value != null)
                {
                    float distance = Vector3.Distance(location, part.Value.Value.shape.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        minCoordinate = part.Key;
                    }
                }
            }

            return minCoordinate;
        }

        public List<AxialCoordinate> findDestroyedPartLocations(AxialCoordinate destroyedPartLocation)
        {
            List<AxialCoordinate> destroyedLocations = new List<AxialCoordinate>();

            // Temporarily destroy the part
            Part? tempPart = getPart(destroyedPartLocation);
            removePart(destroyedPartLocation);

            // Search all parts to see if path is present
            foreach (AxialCoordinate partLocation in dataTable.Keys)
                if (!pathExistsToPlayer(partLocation))
                    destroyedLocations.Add(partLocation);

            // Restore the destroyed part
            addPart(destroyedPartLocation, tempPart);
            destroyedLocations.Add(destroyedPartLocation);

            return destroyedLocations;
        }

        private bool pathExistsToPlayer(AxialCoordinate location)
        {
            // Given a location, find a path that exists from that location back to the player

            List<AxialCoordinate> visited = new List<AxialCoordinate>();
            List<AxialCoordinate> unvisited = new List<AxialCoordinate>();
            visited.Add(location);
            unvisited.AddRange(getFullHexNeighbors(location));

            while(!visited.Contains(player) && unvisited.Count > 0)
            {
                AxialCoordinate nodeToExpand = unvisited[0];
                unvisited.RemoveAt(0);
                visited.Add(nodeToExpand);
                foreach(AxialCoordinate neighbor in getFullHexNeighbors(nodeToExpand))
                    if (!visited.Contains(neighbor)) unvisited.Add(neighbor);
            }

            if (visited.Contains(player)) return true;

            return false;
        }

        public int getRotationFromNeighbor(AxialCoordinate neighborNormal)
        {
            // Given a neighborNormal, return the rotation a triangle should be at
            if (neighborNormal.x == -1 && neighborNormal.y == 0) return 90;
            if (neighborNormal.x == -1 && neighborNormal.y == 1) return 30;
            if (neighborNormal.x == 0 && neighborNormal.y == 1) return 330;
            if (neighborNormal.x == 1 && neighborNormal.y == -1) return 210;
            if (neighborNormal.x == 1 && neighborNormal.y == 0) return 270;
            return 150;
        }

        public AxialCoordinate? getNeighborFromTriangle(AxialCoordinate position)
        {
            // Returns a hexagon attached to the triangle at position. Returns null if error

            if (getPart(position).Value.type != -1) return null;

            int rotation = ((int)(dataTable[position].Value.shape.transform.localRotation.eulerAngles.z) + 180)%360;

            AxialCoordinate neighbor;

            // Given a rotation (ie used for triangle), get the neighbor attached to that part
            if (rotation == 90) neighbor = new AxialCoordinate { x = -1, y = 0 };
            else if (rotation == 30) neighbor = new AxialCoordinate { x = -1, y = 1 };
            else if (rotation == 330) neighbor = new AxialCoordinate { x = 0, y = 1 };
            else if (rotation == 210) neighbor = new AxialCoordinate { x = 1, y = -1 };
            else if (rotation == 270) neighbor = new AxialCoordinate { x = 1, y = 0 };
            else neighbor = new AxialCoordinate { x = 0, y = -1 };

            if (getPart(position + neighbor) != null) return position + neighbor;

            return null;
        }
    }
}
