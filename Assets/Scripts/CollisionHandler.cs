using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class CollisionHandler : Photon.MonoBehaviour
    {

        void OnCollisionEnter2D(Collision2D collision)
        {
			if (collision.collider.gameObject.tag == "Triangle" && photonView.isMine)
            {
				// Getting part adder class
				PartAdder partAdder = GameObject.FindGameObjectWithTag ("PartAdder").GetComponent<PartAdder> ();
				// Gets the axialcoord of the object to destroy
                AxialCoordinate? locationOfObject = partAdder.hexData.getLocation(this.gameObject.transform.position);
				           
                List<AxialCoordinate> listToDestroy = partAdder.hexData.findDestroyedPartLocations((AxialCoordinate)locationOfObject);

                // Update score/points for attacking player
                collision.collider.gameObject.transform.parent.GetComponent<PointScoreHandler>().update(listToDestroy);

                foreach (AxialCoordinate location in listToDestroy)
                    partAdder.removePart(location);
            }
        }
    }
}
