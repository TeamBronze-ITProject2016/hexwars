using UnityEngine;
using System.Collections;

public class DestructibleObjectMovement : MonoBehaviour
{
    public float movespeed;
    public float rotationspeed;

    private float direction;

	void Start()
    {
        if(!PhotonNetwork.isMasterClient)
            return;
        {
            direction = Random.Range(0, 360);
            
        }
	}

    public void SetDirection(float direction)
    {

    }
}
