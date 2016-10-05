using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class DestructibleObjectCollisionHandler : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.tag == "Triangle")
            {
                /* TODO: Add points to player */
                PhotonNetwork.Destroy(gameObject);

                // Update score/points for attacking player
                //collision.collider.gameObject.transform.parent.gameObject.GetComponent<Player>().updateDestructable();
                PhotonView attackingView = PhotonView.Get(collision.collider.gameObject.transform.parent.gameObject);
                attackingView.RPC("updateDestructableServer", PhotonPlayer.Find(attackingView.owner.ID));
            }
        }
    }
}
