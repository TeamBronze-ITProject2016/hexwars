using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
	public class TestNewPartAdder : Photon.MonoBehaviour
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

        public int count = 0;

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
            //InvokeRepeating("AddRandomPartsQueue", 2.0f, 2.0f);
            //InvokeRepeating("AddRandomParts", 2.0f, 2.0f);
            //AddRandomParts();

            InvokeRepeating("unitTest", 2.0f, 2.0f);

        }

        public void unitTest()
        {
            if (playerObj == null)
            {
                playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");
                if (playerObj == null) return;
                GameObject a = AddHexagon(playerObj.gameObject, 0);
                GameObject b = AddHexagon(playerObj.gameObject, 1);
                GameObject c = AddHexagon(b, 2);
            }
        }

        private void printChildren(GameObject obj, string str)
        {
            foreach (KeyValuePair<int, GameObject> entry in obj.GetComponent<HexagonData>().childDict)
            {
                Debug.Log(str + " child = " + entry.Value.GetInstanceID());
            }
        }

        // Adds a random part to player gameObject
		void AddRandomParts(){
            if (playerObj == null)
				playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");
            if (Random.Range(0, 2) == -1) {
                int rand = Random.Range(0, 6);
                Debug.Log("GENPOS " + rand);
                AddTriangle(playerObj, rand);
            }
            else {
                Debug.Log("GENPOS " + count);
                AddHexagon(playerObj, count);
                count += 1; 
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
			float dist = Vector3.Distance (pos, pos + hexEdgeOffsetsWorld [position]);
			Quaternion angle = parent.transform.rotation * Quaternion.Euler(0, 0, 120 - (60 * (position + 1)));
			// angle *= Quaternion.Euler(0, 0, - (60 * (position + 1)));
			float x = 2 * dist * Mathf.Cos(angle.eulerAngles.z * Mathf.Deg2Rad);
            float y = 2 * dist * Mathf.Sin(angle.eulerAngles.z * Mathf.Deg2Rad);
			Vector3 newPosition = pos + new Vector3(x,y,pos.z);

            if (checkOccupied(newPosition))
                return null;

            // Instantiate part at position
			GameObject addedPart = PhotonNetwork.Instantiate(trianglePart.name, newPosition, Quaternion.Euler(new Vector3(0, 0, parent.transform.eulerAngles.z + 30 - 60 * (position + 1))),0);
			//GameObject addedPart = (GameObject)Instantiate(trianglePart, newPosition, Quaternion.Euler(new Vector3(0, 0, parent.transform.eulerAngles.z)));

			// Add part as child of parent hexagon
            addedPart.transform.parent = playerObj.transform;
            // Modifiy Child Hexagon Data
            TriangleData childTriData = addedPart.GetComponent<TriangleData>();
            childTriData.position = position;
            // Modify connecting objects
            updateSurrounding(addedPart);
            // Add part to global gameobject list
            Player playerScript = playerObj.GetComponent<Player>();
            playerScript.partList.Add(addedPart);

            return addedPart;
        }

        // Adds a hexagon gameObject to parent
        public GameObject AddHexagon(GameObject parent, int position)
        {
			if (parent.GetComponent<HexagonData> ().childDict.ContainsKey (position)) {
				Debug.Log ("ALREADY ADDED");
				return null;
			}
			
			Vector3 pos = parent.transform.position;
			// Get position of where to add part
			float dist = Vector3.Distance (pos, pos + hexEdgeOffsetsWorld [position]);
            Debug.Log(dist);
			Quaternion angle = parent.transform.rotation * Quaternion.Euler(0, 0, 120 - (60 * (position + 1)));
			// angle *= Quaternion.Euler(0, 0, - (60 * (position + 1)));
			float x = 2 * dist * Mathf.Cos(angle.eulerAngles.z * Mathf.Deg2Rad);
			float y = 2 * dist * Mathf.Sin(angle.eulerAngles.z * Mathf.Deg2Rad);
			Vector3 newPosition = pos + new Vector3(x,y,pos.z);

            if (checkOccupied(newPosition))
                return null;

			GameObject addedPart = PhotonNetwork.Instantiate(hexagonPart.name, newPosition, Quaternion.Euler(new Vector3(0, 0, parent.transform.eulerAngles.z)),0);
			//GameObject addedPart = (GameObject)Instantiate(hexagonPart, newPosition, Quaternion.Euler(new Vector3(0, 0, parent.transform.eulerAngles.z)));

			// Add part as child of parent hexagon
            addedPart.transform.parent = playerObj.transform;
            // Modifiy Child Hexagon Data
            HexagonData hexData = addedPart.GetComponent<HexagonData>();
            hexData.position = position;
            // Modify connecting objects
            updateSurrounding(addedPart);
            // Add part to global gameobject list
            Player playerScript = playerObj.GetComponent<Player>();
            playerScript.partList.Add(addedPart);

            return addedPart;
        }

        // Search all objects remaining to see if a path from player to object is present
        // If path is not present, object should be destoryed  
        public List<GameObject> findDestroyedObjects(GameObject destroyedObj)
        {
            List<GameObject> destroyedObjects = new List<GameObject>();
            // Search all objects to see if path is present
            foreach (GameObject obj in playerObj.GetComponent<Player>().partList)
            {
                if (!findPathBFS(destroyedObj, obj))
                    destroyedObjects.Add(obj);
            }
            return destroyedObjects;
        }

        // Use BFS to find path from player to gameobject, ignoring the destroyed object
        private bool findPathBFS(GameObject destroyedObj, GameObject searchObj)
        {
            // Visited dictionary
            Dictionary<GameObject, bool> visited = new Dictionary<GameObject, bool>();
            // Add all objects except destoryed object to visited
            foreach (GameObject obj in playerObj.GetComponent<Player>().partList)
            {
                if (obj != destroyedObj)
                    visited.Add(obj, false);
            }

            Queue<GameObject> queue = new Queue<GameObject>();

            // Start at playerObj origin
            visited[playerObj] = true;
            queue.Enqueue(playerObj);

            // Search using BFS until all parts are visited
            while(queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                foreach (KeyValuePair<int, GameObject> entry in obj.GetComponent<HexagonData>().childDict)
                {
                    if (entry.Value == destroyedObj)
                        continue;

                    // If path is found, exit
                    if (entry.Value == searchObj)
                        return true;

                    // Add node to queue, ignore visted nodes
                    if (!visited[entry.Value])
                    {
                        visited[entry.Value] = true;
                        // Ignore triangles
                        if (entry.Value.tag != "triangle")
                            queue.Enqueue(entry.Value);
                    }
                }
            }
            return false;
        }

        // Finds all surrounding gameobjects and updates their dictionaries
        private void updateSurrounding(GameObject newObject)
        {
            foreach (GameObject obj in playerObj.GetComponent<Player>().partList)
            {
                Debug.Log("dist= " + Vector3.Distance(newObject.transform.position, obj.transform.position));
                // Updates all surrounding objects
                if (Vector3.Distance(newObject.transform.position, obj.transform.position) < 2.3f)
                {
                    int pos = getPosFromAngle(obj.transform.position, newObject.transform.position);
                    Debug.Log("POS IS " + pos);
                    // Update origin player
                    if (obj.tag == "Hexagon" || obj.tag == "LocalPlayer")
                    {
                        obj.GetComponent<HexagonData>().childDict.Add(pos, newObject);
                    }
                    // Update added player
                    if (newObject.tag == "Hexagon") {
                        pos = getOppositePos(pos);
                        newObject.GetComponent<HexagonData>().childDict.Add(pos, obj);
                    }
                }
            }
            return;
        }
        
        // Gets position using angle from two vectors
        private int getPosFromAngle(Vector3 posOrigin, Vector3 posNew)
        {
            // Get angle counterclockwise from vector3.up
			float angle = Quaternion.FromToRotation(Vector3.up, posNew - posOrigin).eulerAngles.z - playerObj.transform.eulerAngles.z;
			angle = normalizeAngle (angle);
			Debug.Log ("angle = " + angle);
            if (angle <= 60) return 5;
            else if (angle <= 120) return 4;
            else if (angle <= 180) return 3;
            else if (angle <= 240) return 2 ;
            else if (angle <= 300) return 1;
            else return 0;
        }

		private float normalizeAngle(float angle){
			angle = angle % 360;
			if (angle < 0) {
				angle += 360;
			}
			return angle;
		}

        // Gets the opposite position from int
        public int getOppositePos(int pos)
        {
            if (pos == 5) return 2;
            else if (pos == 4) return 1;
            else if (pos == 3) return 0;
            else if (pos == 2) return 5;
            else if (pos == 1) return 4;
            else return 3;
        }

        // Checks if position newPos is already occupied
        bool checkOccupied(Vector3 newPos)
        {
            foreach (GameObject obj in playerObj.GetComponent<Player>().partList)
            {
                // Updates all surrounding objects
                if (Vector3.Distance(newPos, obj.transform.position) < 1.5*horizontalParallelOffsetWorld)
                {
					Debug.Log ("OCCUPIED");
                    return true;
                }
            }
            return false;
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
            for (int i =0; i<3; i++)
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