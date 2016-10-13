using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class EnemyCollisionHandler : Photon.MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Make sure collision is with core part
            if (!GetComponent<PolygonCollider2D>().IsTouching(collision.collider))
                return;

            GameObject collisionObj = collision.collider.gameObject;

            // Destroy on player collision or collision with another AI
            if ((collisionObj.tag == "Triangle" && collisionObj.GetPhotonView().isMine) || collisionObj.tag == "EnemyTriangle")
            {
                photonView.RPC("PunDestroy", PhotonTargets.MasterClient);

                // Add points only for player collision
                if (collisionObj.tag == "Triangle" && collisionObj.GetPhotonView().isMine)
                {
                    // Add points to player
                }
            }
        }

        [PunRPC]
        void PunDestroy()
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
