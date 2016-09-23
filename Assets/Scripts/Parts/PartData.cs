﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{

    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public struct Part
    {
        public GameObject shape;
        public int type;
    }

    public struct AxialCoordinate
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
            if (pathExistsToPlayer(location) && ((dataTable.ContainsKey(location) && dataTable[location] == null) || (!dataTable.ContainsKey(location))))
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

        /*public List<Part?> getParts()
        {
            List<Part?> items = new List<Part?>();
            items.AddRange(dataTable.Values);

            return items;
        }*/

        public List<AxialCoordinate> getEmptyNeighbors(AxialCoordinate location)
        {
            List<AxialCoordinate> empty = new List<AxialCoordinate>();
            foreach (AxialCoordinate direction in directions)
            {
                AxialCoordinate neighbor = new AxialCoordinate
                { x = location.x + direction.x, y = location.y + direction.y };
                if (getPart(neighbor) == null)
                    empty.Add(neighbor);
            }
            return empty;
        }

        public List<AxialCoordinate> getFullNeighbors(AxialCoordinate location)
        {
            List<AxialCoordinate> full = new List<AxialCoordinate>();
            
            foreach (AxialCoordinate direction in directions)
            {
                AxialCoordinate neighbor = new AxialCoordinate
                { x = location.x + direction.x, y = location.y + direction.y };
                if (getPart(neighbor) != null)
                    full.Add(neighbor);
            }

            return full;
        }

        public int removePart(AxialCoordinate location)
        {
            // Remove a part, return the type of the part that was removed
            int type = dataTable[location].Value.type;

            if (location == player)
            {
                // Player is dead!
                dataTable = new Dictionary<AxialCoordinate, Part?>();
                return 0;
            }

            dataTable[location] = null;

            return type;
        }

        private bool pathExistsToPlayer(AxialCoordinate location)
        {
            // Given a location, find a path that exists from that location back to the player
            // TODO
            return true;
        }
    }
}