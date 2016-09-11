using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class PartAdder : MonoBehaviour
    {
        // Stores the prefabs for each gameobject part
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
            //GameObject player = GameObject.Find("Player");
            //AddRandomParts(player.GetComponent<Player>());
            //GameObject player = GameObject.FindGameObjectWithTag("LocalPlayer");
            //Debug.Log (player);
            //playerObj = player;
            //InvokeRepeating("AddRandomParts", 2.0f, 2.0f);
            //AddPart(playerObj);
            InvokeRepeating("AddRandomPartsQueue", 2.0f, 2.0f);
        }

        // Adds a random part to player gameObject
        void AddRandomParts()
        {
            if (playerObj == null)
                playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");
            if (Random.Range(0, 2) == -1)
                AddTriangle(playerObj, Random.Range(0, 6));
            else
            {
                AddHexagon(playerObj, Random.Range(0, 6));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Adds a triangle gameObject to parent
        GameObject AddTriangle(GameObject parent, int position)
        {
            if (parent.GetComponent<HexagonData>().childDict.ContainsKey(position))
                return null;

            Vector3 pos = parent.transform.position;
            // Get position of where to add part
            float dist = Vector3.Distance(pos, pos + hexEdgeOffsetsWorld[position]);
            Quaternion angle = parent.transform.rotation * Quaternion.Euler(0, 0, 120 - (60 * (position + 1)));
            // angle *= Quaternion.Euler(0, 0, - (60 * (position + 1)));
            float x = dist * Mathf.Cos(angle.eulerAngles.z * Mathf.Deg2Rad);
            float y = dist * Mathf.Sin(angle.eulerAngles.z * Mathf.Deg2Rad);
            Vector3 newPosition = pos + new Vector3(x, y, pos.z);
            Vector3 checkPosition = pos + new Vector3(2 * x, 2 * y, pos.z);

            if (checkOccupied(checkPosition, playerObj))
                return null;

            // Instantiate part at position
            GameObject addedPart = (GameObject)Instantiate(trianglePart, newPosition, Quaternion.Euler(new Vector3(0, 0, parent.transform.eulerAngles.z + 30 - 60 * (position + 1))));

            // Add part as child of parent hexagon
            addedPart.transform.parent = parent.gameObject.transform;
            // Modifiy Child Hexagon Data
            TriangleData childTriData = addedPart.GetComponent<TriangleData>();
            childTriData.parent = parent;
            childTriData.position = position;
            Debug.Log("added at " + position);
            // Modify Parent Hexagon Data
            HexagonData parHexData = parent.GetComponent<HexagonData>();
            parHexData.childDict.Add(position, addedPart);

            return addedPart;
        }

        // Adds a hexagon gameObject to parent
        GameObject AddHexagon(GameObject parent, int position)
        {
            if (parent.GetComponent<HexagonData>().childDict.ContainsKey(position))
                return null;

            Vector3 pos = parent.transform.position;
            // Get position of where to add part
            float dist = Vector3.Distance(pos, pos + hexEdgeOffsetsWorld[position]);
            Quaternion angle = parent.transform.rotation * Quaternion.Euler(0, 0, 120 - (60 * (position + 1)));
            // angle *= Quaternion.Euler(0, 0, - (60 * (position + 1)));
            float x = 2 * dist * Mathf.Cos(angle.eulerAngles.z * Mathf.Deg2Rad);
            float y = 2 * dist * Mathf.Sin(angle.eulerAngles.z * Mathf.Deg2Rad);
            Vector3 newPosition = pos + new Vector3(x, y, pos.z);

            if (checkOccupied(newPosition, playerObj))
                return null;

            GameObject addedPart = (GameObject)Instantiate(hexagonPart, newPosition, Quaternion.Euler(new Vector3(0, 0, parent.transform.eulerAngles.z)));
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

        // Checks if position newPos is already occupied
        bool checkOccupied(Vector3 newPos, GameObject parent)
        {
            Vector3 childPos = parent.transform.position;
            // Modified the position if parent gameobject is triangle
            // This is because the pos point of the triangle is at the bottom edge of the triangle
            if (parent.tag == "Triangle")
            {
                Vector3 pos = parent.transform.position;
                float dist = Vector3.Distance(pos, pos + hexEdgeOffsetsWorld[parent.GetComponent<TriangleData>().position]);
                Quaternion angle = playerObj.transform.rotation * Quaternion.Euler(0, 0, 120 - (60 * (parent.GetComponent<TriangleData>().position + 1)));
                float x = dist * Mathf.Cos(angle.eulerAngles.z * Mathf.Deg2Rad);
                float y = dist * Mathf.Sin(angle.eulerAngles.z * Mathf.Deg2Rad);
                childPos = pos + new Vector3(x, y, pos.z);
            }
            // Checks if position to add new part is too close to another gameobject
            if (Vector3.Distance(newPos, childPos) < 1)
            {
                return true;
            }
            // Recursively checks all gameobject to see if position is occupied
            bool retval = false;
            if (parent.tag == "Hexagon" || parent.tag == "LocalPlayer")
            {
                foreach (KeyValuePair<int, GameObject> entry in parent.GetComponent<HexagonData>().childDict)
                {
                    retval = retval || checkOccupied(newPos, entry.Value);
                }
            }
            return retval;
        }

        // Finds an empty spot for the hexagon
        GameObject FindEmpty(GameObject parent)
        {
            Queue queue = new Queue();
            queue.Enqueue(parent);
            while (queue.Count > 0)
            {
                GameObject obj = (GameObject)queue.Dequeue();
                Debug.Log(obj.tag);
                if (obj.tag == "Hexagon" || obj.tag == "LocalPlayer")
                {
                    foreach (KeyValuePair<int, GameObject> entry in obj.GetComponent<HexagonData>().childDict)
                    {
                        if (entry.Value.tag == "Hexagon")
                        {
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
            if (playerObj == null)
                playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");
            GameObject obj = FindEmpty(playerObj);
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
            for (int i = 0; i < 3; i++)
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