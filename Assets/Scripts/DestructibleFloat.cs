using UnityEngine;
using System.Collections;

public class DestructibleFloat : MonoBehaviour {

	public float FloatStrength;

	// Use this for initialization
	void Start () {
	
	}

	void Update () {
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D> ();
		rb.AddForce(new Vector3(Random.Range(-FloatStrength,FloatStrength), Random.Range(-FloatStrength,FloatStrength), 0));
	}
}
