using UnityEngine;
using System.Collections;
using System;

namespace TeamBronze.HexWars
{
    public class HexagonPart : Part
    {
        public Part[] partList = new Part[3];
        public GameObject hexagonPart;

        public HexagonPart(GameObject parent, int position) : base(parent, position)
        {
        }

        internal void init(GameObject parent, int position)
        {
            Debug.Log(hexEdgeOffsets[0]);
            Vector3 pos = parent.transform.position;
            // Get position of where to add part
            pos = Camera.main.WorldToScreenPoint(pos) + 2 * hexEdgeOffsets[position];
            // Instantiate part at position
            GameObject addedPart = (GameObject)Instantiate(hexagonPart, Camera.main.ScreenToWorldPoint(pos), Quaternion.Euler(new Vector3(0, 0, 30 - 60 * (position + 1))));
            // Add part as child of parent hexagon
            addedPart.transform.parent = parent.gameObject.transform;
            // Add gameObject
            this.thisPart = addedPart;
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}