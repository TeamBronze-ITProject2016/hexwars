using UnityEngine;
using System.Collections;

public class PartAdder : MonoBehaviour {

    public GameObject attackingPart;
    // Check is edge is occupied
    private bool[] edges = { true, true, true, true, true, true };
    // Store pixel locations of where part is to be attached
    private Vector3[] partPos = { new Vector3(64/2, 93/2), new Vector3(128/2, 0), new Vector3(64/2, -93/2), new Vector3(-64/2, -93/2), new Vector3(-128/2, 0), new Vector3(-64/2, 93/2) };

	// Use this for initialization
	void Start () {
        // Test adding 3 parts
        AddPart(2);
        AddPart(1);
        AddPart(3);
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
        GameObject addedPart = (GameObject)Instantiate(attackingPart, Camera.main.ScreenToWorldPoint(pos), Quaternion.Euler(new Vector3(0, 0, 30 - 60 * (partNumber + 1))));
        // Add part as child of parent hexagon
        addedPart.transform.parent = player.gameObject.transform;
    }
}
