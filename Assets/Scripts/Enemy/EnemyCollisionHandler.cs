/* EnemyCollisionHandler.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Handles collisons with enemies
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class EnemyCollisionHandler : Photon.MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Make sure collision is with core part and not a triangle
            if (!GetComponent<PolygonCollider2D>().IsTouching(collision.collider))
                return;

            GameObject collisionObj = collision.collider.gameObject;

            // Destroy on player collision or collision with another AI
            if ((collisionObj.tag == "Triangle" && collisionObj.GetPhotonView().isMine) || collisionObj.tag == "EnemyTriangle")
            {
                // Request master client to destroy this enemy
                photonView.RPC("PunDestroy", PhotonTargets.MasterClient);

                // Add points only for player collision
                if (collisionObj.tag == "Triangle" && collisionObj.GetPhotonView().isMine)
                {
                    PhotonView attackingView = PhotonView.Get(collision.collider.gameObject.transform.parent.gameObject);
                    //attackingView.RPC("updateLocalPointsEnemy", PhotonPlayer.Find(attackingView.owner.ID));
                }
            }
        }

        // Destroys an enemy over PUN. Clients should request the master client to call this in order to
        // destroy an enemy.
        [PunRPC]
        void PunDestroy()
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
