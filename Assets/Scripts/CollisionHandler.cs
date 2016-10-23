/* CollisionHandler.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Handles collisions with player hexagons.
 */

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
            // Each player handles their own collisions, so only handle collisions for the local player
            if(!photonView.isMine)
                return;

            // Check if collision was with a triangle part, either a player's or an AI enemy's
			if (collision.collider.gameObject.tag == "Triangle" || collision.collider.gameObject.tag == "EnemyAttackingPart")
            {
				// Getting part adder class
				PartAdder partAdder = GameObject.FindGameObjectWithTag ("PartAdder").GetComponent<PartAdder> ();
				// Gets the axialcoord of the object to destroy
                AxialCoordinate locationOfObject = partAdder.hexData.getLocation(this.gameObject.transform.position);
				           
                List<AxialCoordinate> listToDestroy = partAdder.hexData.findDestroyedPartLocations(locationOfObject);

                // Only add points to attacker if attacked by player, not AI enemy
                if (collision.collider.gameObject.tag == "Triangle")
                {
                    // Update score/points for attacking player
                    PhotonView attackingView = PhotonView.Get(collision.collider.gameObject.transform.parent.gameObject);

                    // Convert list to string data stream, as Photon cannot serialize lists
                    var binFormatter = new BinaryFormatter();
                    var mStream = new MemoryStream();
                    binFormatter.Serialize(mStream, listToDestroy);
                    var data = Convert.ToBase64String(mStream.GetBuffer());

                    // Send data to attacking player using RPC call
                    attackingView.RPC("updateLocalPointsList", PhotonPlayer.Find(attackingView.owner.ID), data);

                }

                // Destroy appropriate parts for the collision
                foreach (AxialCoordinate location in listToDestroy)
                    partAdder.removePart(location);
            }
        }
    }
}
