using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class LocalDestructibleObjectCollisionHandler : Photon.MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.tag == "Triangle")
            {
                GetComponent<LocalObjectFader>().FadeOut();

                // Update points for attacking player
                PhotonView attackingView = PhotonView.Get(collision.collider.gameObject.transform.parent.gameObject);
                attackingView.RPC("updateLocalPointsDestructable", PhotonPlayer.Find(attackingView.owner.ID));
            }
        }
    }
}
