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
			if (photonView.isMine && (collision.collider.gameObject.tag == "Triangle" || collision.collider.gameObject.tag == "EnemyAttackingPart"))
            {
				// Getting part adder class
				PartAdder partAdder = GameObject.FindGameObjectWithTag ("PartAdder").GetComponent<PartAdder> ();
				// Gets the axialcoord of the object to destroy
                AxialCoordinate locationOfObject = partAdder.hexData.getLocation(this.gameObject.transform.position);
				           
                List<AxialCoordinate> listToDestroy = partAdder.hexData.findDestroyedPartLocations(locationOfObject);

                // Only if attacked by player, not AI enemy
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

                foreach (AxialCoordinate location in listToDestroy)
                    partAdder.removePart(location);

                // Update the scoreboard
                GameObject scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard");
                PhotonView scoreboardView = PhotonView.Get(scoreboard);
                scoreboardView.RPC("UpdateScoresBoard", PhotonTargets.All);
            }
        }
    }
}
