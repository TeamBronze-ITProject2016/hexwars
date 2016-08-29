using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class TestNewPartAdder : MonoBehaviour
    {
        public GameObject playerObj;
        public GameObject hexagonPart;
        public GameObject trianglePart;

        public static float horiztonalDiagOffsetWorld = 0.56F;
        public static float verticalDiagOffsetWorld = 0.92F;

        public static float horizontalParallelOffsetWorld = 1.1F;
        public static float verticalParallelOffsetWorld = 0F;

        public Vector3[] hexEdgeOffsetsWorld;

        // Use this for initialization
        void Start()
        {
            hexEdgeOffsetsWorld = new Vector3[] {
            new Vector3(horiztonalDiagOffsetWorld, verticalDiagOffsetWorld),
            new Vector3(horizontalParallelOffsetWorld, verticalParallelOffsetWorld),
            new Vector3(horiztonalDiagOffsetWorld, -verticalDiagOffsetWorld),
            new Vector3(-horiztonalDiagOffsetWorld, -verticalDiagOffsetWorld),
            new Vector3(-horizontalParallelOffsetWorld, verticalParallelOffsetWorld),
            new Vector3(-horiztonalDiagOffsetWorld, verticalDiagOffsetWorld)
        };
            Debug.Log(hexEdgeOffsetsWorld[0]);
            //GameObject player = GameObject.Find("Player");
            //AddRandomParts(player.GetComponent<Player>());
            GameObject player = GameObject.Find("Player");
            playerObj = player;
            InvokeRepeating("AddRandomPartsQueue", 2.0f, 2.0f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        GameObject AddTriangle(GameObject parent, int position)
        {
            Vector3 pos = parent.transform.position;
            // Get position of where to add part
            //Debug.Log(pos);
            pos = pos + hexEdgeOffsetsWorld[position];
            //Debug.Log(pos);
            //Debug.Log(hexEdgeOffsetsWorld[position]);
            // Instantiate part at position
            GameObject addedPart = (GameObject)Instantiate(trianglePart, pos, Quaternion.Euler(new Vector3(0, 0, parent.transform.rotation.z + 30 - 60 * (position + 1))));
            // Add part as child of parent hexagon
            addedPart.transform.parent = parent.gameObject.transform;
            // Modifiy Child Hexagon Data
            TriangleData childTriData = addedPart.GetComponent<TriangleData>();
            childTriData.parent = parent;
            childTriData.position = position;
            // Modify Parent Hexagon Data
            HexagonData parHexData = parent.GetComponent<HexagonData>();
            parHexData.childDict.Add(position, addedPart);

            return addedPart;
        }

        GameObject AddHexagon(GameObject parent, int position)
        {
            Vector3 pos = parent.transform.position;
            // Get position of where to add part
            //Debug.Log(pos);
            pos = pos + 2 * hexEdgeOffsetsWorld[position];
            //Debug.Log(pos);
            //Debug.Log(hexEdgeOffsetsWorld[position]);
            // Instantiate part at position
            GameObject addedPart = (GameObject)Instantiate(hexagonPart, pos, Quaternion.Euler(new Vector3(0, 0, parent.transform.rotation.z)));
            // Add part as child of parent hexagon
            addedPart.transform.parent = parent.gameObject.transform;
            // Modifiy Child Hexagon Data
            HexagonData hexData = addedPart.GetComponent<HexagonData>();
            hexData.parent = parent;
            hexData.position = position;
            // Modify Parent Hexagon Data
            HexagonData parHexData = parent.GetComponent<HexagonData>();
            parHexData.childDict.Add(position, addedPart);

            return addedPart;
        }

        GameObject FindEmpty(GameObject parent)
        {
            Queue queue = new Queue();
            queue.Enqueue(parent);
            while (queue.Count > 0)
            {
                GameObject obj = (GameObject)queue.Dequeue();
                Debug.Log(obj.tag);
                if (obj.tag == "Hexagon")
                {
                    foreach (KeyValuePair<int, GameObject> entry in obj.GetComponent<HexagonData>().childDict)
                    {
                        if (entry.Value.tag == "Hexagon")
                        {
                            Debug.Log("HEXFOUND");
                            queue.Enqueue(entry.Value);
                        }
                    }
                    if (obj.GetComponent<HexagonData>().childDict.Count < 3)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

        void AddRandomPartsQueue()
        {
            GameObject obj = playerObj; /* FindEmpty(playerObj);*/
            if (obj) AddPart(obj);
            else
            {
                Debug.Log("WTF");
            }
        }

        void AddRandomParts(Player player)
        {
            AddPart(player.gameObject);
        }

        // No comments, will fix later
        void AddPart(GameObject parent)
        {
            Debug.Log(hexEdgeOffsetsWorld);
            // Modify Parent Hexagon Data
            HexagonData parHexData = parent.GetComponent<HexagonData>();
            while (parHexData.childDict.Count < 3)
            {
                if (Random.Range(0, 2) == 0)
                {
                    int key;
                    do
                    {
                        key = Random.Range(0, 6);
                    } while (parHexData.childDict.ContainsKey(key));
                    GameObject addedPart = AddHexagon(parHexData.gameObject, key);
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