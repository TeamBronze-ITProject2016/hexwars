/*PointsCollectable.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Collectable object which gives the player points for collecting it.*/
 
using UnityEngine;
using System.Collections;

/*
    -Should be released after a player or a player's parts have been destroyed.
    -Should follow the player within some radius.
    -Once collected, should add points to the player who collected it and release 
    particles.
*/

public class PointsCollectable : MonoBehaviour {

    ParticleSystem particleSystem;

    private float points = 0.0f;

    /*The lifetime of this object*/
    private float lifetime = 10.0f;
    private float timeCreated = -1.0f;

    private int particleCount = 0;

	/*Initialise*/
	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        Debug.Assert(particleSystem);

        particleCount = particleSystem.particleCount;
	}
	
	/*Called once per frame.*/
	void Update () {
	    if(timeCreated == -1.0f){
            timeCreated = Time.time;
        }

        float lifetimePercent = (Time.time - timeCreated)/lifetime;

        if (lifetimePercent > 1.0f)
            Destroy(this);
	}

    /*Destroys this object and return the number of points it is worth*/
    public float collect()
    {
        Destroy(this);
        return points;
    }
}
