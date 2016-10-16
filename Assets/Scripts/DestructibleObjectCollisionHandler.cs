using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class DestructibleObjectCollisionHandler : Photon.MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.tag == "Triangle")
            {
				photonView.RPC ("PunFadeOut", PhotonTargets.All);

                // Update points for attacking player
                PhotonView attackingView = PhotonView.Get(collision.collider.gameObject.transform.parent.gameObject);
                attackingView.RPC("updateLocalPointsDestructable", PhotonPlayer.Find(attackingView.owner.ID));

                // Update the scoreboard
                PhotonView scoreboardView = PhotonView.Get(GameObject.FindGameObjectWithTag("ScoreBoard"));
                scoreboardView.RPC("UpdateScoresBoard", PhotonTargets.All);
            }
        }
    }
}
