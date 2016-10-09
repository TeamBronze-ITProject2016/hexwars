using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
                AxialCoordinate locationOfObject = partAdder.hexData.getLocation(this.gameObject.transform.position);
				           
                List<AxialCoordinate> listToDestroy = partAdder.hexData.findDestroyedPartLocations(locationOfObject);

                // Update score/points for attacking player
                PhotonView attackingView = PhotonView.Get(collision.collider.gameObject.transform.parent.gameObject);

                // Convert list to string data stream, as Photon cannot serialize lists
                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();
                binFormatter.Serialize(mStream, listToDestroy);
                var data = Convert.ToBase64String(mStream.GetBuffer());

                // Send data to attacking player using RPC call
                attackingView.RPC("updateLocalPointsList", PhotonPlayer.Find(attackingView.owner.ID), data);

                /* TODO: Decide whether to do this locally or through server */
                // Update score for destroyed player
                // PhotonView destroyedView = PhotonView.Get(this);
                // destroyedView.RPC("updateServerScore", PhotonPlayer.Find(destroyedView.owner.ID));

                foreach (AxialCoordinate location in listToDestroy)
                    partAdder.removePart(location);
            }
        }
    }
}
