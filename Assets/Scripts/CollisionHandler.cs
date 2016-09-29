using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class CollisionHandler : Photon.MonoBehaviour
    {

        void OnCollisionEnter2D(Collision2D collision)
        {
			if (collision.collider.gameObject.tag == "Triangle")
            {
				PartAdder partAdder = GameObject.FindGameObjectWithTag ("PartAdder").GetComponent<PartAdder> ();
                AxialCoordinate? locationOfObject = partAdder.hexData.getLocation(this.gameObject.transform.position);

                if (locationOfObject != null) // If null, something's wrong
                {
                    List<AxialCoordinate> listToDestroy = partAdder.hexData.findDestroyedPartLocations((AxialCoordinate)locationOfObject);
                    Debug.Log("listToDestroyXXX" + listToDestroy);
                    foreach (AxialCoordinate location in listToDestroy)
                    {
                        Debug.Log("YAAAAAAAAAS");
                        partAdder.removePart(location);
                    }
                }
            }
        }
    }
}
