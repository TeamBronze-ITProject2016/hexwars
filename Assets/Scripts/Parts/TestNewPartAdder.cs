using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class TestNewPartAdder : MonoBehaviour
    {

        public GameObject hexagonPart;
        public GameObject trianglePart;

        public static int horizontalDiagOffset = 29;
        public static int verticalDiagOffset = 47;

        public static int horizontalParallelOffset = 56;
        public static int verticalParallelOffset = 0;

        // Store pixel locations of where part is to be attached
        public Vector3[] hexEdgeOffsets = {
        new Vector3(horizontalDiagOffset, verticalDiagOffset),
        new Vector3(horizontalParallelOffset, verticalParallelOffset),
        new Vector3(horizontalDiagOffset, -verticalDiagOffset),
        new Vector3(-horizontalDiagOffset, -verticalDiagOffset),
        new Vector3(-horizontalParallelOffset, verticalParallelOffset),
        new Vector3(-horizontalDiagOffset, verticalDiagOffset)
    };

        // Use this for initialization
        void Start()
        {
            GameObject player = GameObject.Find("Player");
            AddRandomParts(player.GetComponent<Player>());
            //AddRandomPart(startHex);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void AddTriangle(GameObject parent, int position)
        {
            Vector3 pos = parent.transform.position;
            // Get position of where to add part
            pos = Camera.main.WorldToScreenPoint(pos) + hexEdgeOffsets[position];
            // Instantiate part at position
            GameObject addedPart = (GameObject)Instantiate(trianglePart, Camera.main.ScreenToWorldPoint(pos), Quaternion.Euler(new Vector3(0, 0, 30 - 60 * (position + 1))));
            // Add part as child of parent hexagon
            addedPart.transform.parent = parent.gameObject.transform;
            // Modifiy Child Hexagon Data
            TriangleData childTriData = addedPart.GetComponent<TriangleData>();
            childTriData.parent = parent;
            childTriData.position = position;
            // Modify Parent Hexagon Data
            HexagonData parHexData = parent.GetComponent<HexagonData>();
            if (parHexData == null)
            {
                Player parPlayerData = parent.GetComponent<Player>();
                parPlayerData.childDict.Add(position, addedPart);
            }
            else
            {
                parHexData.childDict.Add(position, addedPart);
            }
        }

        void AddHexagon(GameObject parent, int position)
        {
            Vector3 pos = parent.transform.position;
            // Get position of where to add part
            pos = Camera.main.WorldToScreenPoint(pos) + 2 * hexEdgeOffsets[position];
            // Instantiate part at position
            GameObject addedPart = (GameObject)Instantiate(hexagonPart, Camera.main.ScreenToWorldPoint(pos), Quaternion.identity);
            // Add part as child of parent hexagon
            addedPart.transform.parent = parent.gameObject.transform;
            // Modifiy Child Hexagon Data
            HexagonData hexData = addedPart.GetComponent<HexagonData>();
            hexData.parent = parent;
            hexData.position = position;
            // Modify Parent Hexagon Data
            HexagonData parHexData = parent.GetComponent<HexagonData>();
            if (parHexData == null)
            {
                Player parPlayerData = parent.GetComponent<Player>();
                parPlayerData.childDict.Add(position, addedPart);
            }
            else
            {
                parHexData.childDict.Add(position, addedPart);
            }
        }

        void AddRandomParts(Player player)
        {
            AddPart(player.gameObject);
        }

        // No comments, will fix later
        void AddPart(GameObject parent)
        {
            // Modify Parent Hexagon Data
            HexagonData parHexData = parent.GetComponent<HexagonData>();
            if (parHexData == null)
            {
                Player parPlayerData = parent.GetComponent<Player>();
                while (parPlayerData.childDict.Count != 3)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        int key;
                        do
                        {
                            key = Random.Range(0, 6);
                        } while (parPlayerData.childDict.ContainsKey(key));
                        AddHexagon(parPlayerData.gameObject, key);
                    }
                    else
                    {
                        int key;
                        do
                        {
                            key = Random.Range(0, 6);
                        } while (parPlayerData.childDict.ContainsKey(key));
                        AddTriangle(parPlayerData.gameObject, key);
                    }
                }
            }
            else
            {
                while (parHexData.childDict.Count != 3)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        int key;
                        do
                        {
                            key = Random.Range(0, 6);
                        } while (parHexData.childDict.ContainsKey(key));
                        AddHexagon(parHexData.gameObject, key);
                    }
                    else
                    {
                        int key;
                        do
                        {
                            key = Random.Range(0, 6);
                        } while (parHexData.childDict.ContainsKey(key));
                        AddTriangle(parHexData.gameObject, key);
                    }
                }
            }
        }
    }
}