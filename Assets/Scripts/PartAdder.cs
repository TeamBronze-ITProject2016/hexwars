﻿using UnityEngine;
using System.Collections;

public class PartAdder : MonoBehaviour {

    public GameObject attackingPart;
    // Check is edge is occupied
    private bool[] edges = { true, true, true, true, true, true };
    // Store pixel locations of where part is to be attached
    private Vector3[] partPos = { new Vector3(64/2, 93/2), new Vector3(128/2, 0), new Vector3(64/2, -93/2), new Vector3(-64/2, -93/2), new Vector3(-128/2, 0), new Vector3(-64/2, 93/2) };

	// Use this for initialization
	void Start () {
        AddPart(2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Add part to edge
    void AddPart(int partNumber) {
        // Get player position
        GameObject player = GameObject.Find("Player");
        Vector3 pos = player.transform.position;
        // Get position of where to add part
        pos = Camera.main.WorldToScreenPoint(pos) + partPos[partNumber]; 
        // Instantiate part at position
        Instantiate(attackingPart, Camera.main.ScreenToWorldPoint(pos), Quaternion.Euler(new Vector3(0, 0, 30 - 60 * (partNumber + 1))));

        Debug.Log(pos);
        //attackingPart.transform.parent = player.gameObject.transform;
    }
}
