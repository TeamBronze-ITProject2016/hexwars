using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexagonData : MonoBehaviour {

    // Stores parent gameObject
    public GameObject parent;
    // Stores children gameObjects
    public Dictionary<int, GameObject> childDict = new Dictionary<int, GameObject>();
    // Position of this gameObject
    public int position;

	// Use this for initialization
	void Start () {
	        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
