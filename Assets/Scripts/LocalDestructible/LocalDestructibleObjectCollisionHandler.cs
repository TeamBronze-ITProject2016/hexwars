using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class LocalDestructibleObjectCollisionHandler : Photon.MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject collisionObj = collision.collider.gameObject;

            if (collisionObj.tag == "Triangle" || collisionObj.tag == "EnemyAttackingPart")
            {
                GetComponent<LocalObjectFader>().FadeOut();

                // Update points if local player destroyed
                GameObject playerObj = collisionObj.transform.parent.gameObject;
                if (playerObj.tag == "LocalPlayer")
                    playerObj.GetComponent<Player>().points += 1;
            }
        }
    }
}
