using UnityEngine;
using System.Collections;

public class DestroyEffectHandler : MonoBehaviour {

	public GameObject particleEffect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy (){
		Instantiate (particleEffect, transform.position, transform.rotation);
	}
}
