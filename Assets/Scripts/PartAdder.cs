using UnityEngine;
using System.Collections;

public class PartAdder : MonoBehaviour {

    public GameObject attackingPart;
    private bool[] edges = { true, true, true, true, true, true };
    private Vector3[] partPos = { new Vector3(64/2, 93/2), new Vector3(128/2, 0), new Vector3(64/2, -93/2), new Vector3(-64/2, -93/2), new Vector3(-128/2, 0), new Vector3(-64/2, 93/2) };

	// Use this for initialization
	void Start () {
        AddPart(2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AddPart(int partNumber) {
        GameObject player = GameObject.Find("Player");
        Vector3 pos = player.transform.position;
        pos = Camera.main.WorldToScreenPoint(pos) + partPos[partNumber]; 
        Instantiate(attackingPart, Camera.main.ScreenToWorldPoint(pos), Quaternion.Euler(new Vector3(0, 0, 30 - 60 * (partNumber + 1))));

        Debug.Log(pos);
        //attackingPart.transform.parent = player.gameObject.transform;
    }
}
