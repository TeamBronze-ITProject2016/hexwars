/* GameOverHandler.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Collision handler for the player's central part, will cause game over
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

namespace TeamBronze.HexWars
{
    public class GameOverHandler : Photon.PunBehaviour
    {
        // Reference to the game manager object
        private GameManager gameManager;

        // Reference to the scoreboard object
        private Scoreboard scoreboard;

        // Initialize
        void Start()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<Scoreboard>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Only handle collisions with local player
            if (!photonView.isMine)
                return;

            // Make sure collision is with core part
            if (!GetComponent<PolygonCollider2D>().IsTouching(collision.collider))
                return;

            string collisionObjTag = collision.collider.gameObject.tag;

            // Check if collision was with a triangle part
            if(collisionObjTag == "Triangle" || collisionObjTag ==  "EnemyAttackingPart")
            {
                // Game over! Save player's highest score, destroy all objects owned by this player, and disconnect
                float highestScore = scoreboard.GetLocalPlayerHighestScore();
                PlayerPrefs.SetFloat("highestScore", highestScore);
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player.ID);
                gameManager.isGameOver = true;
                PhotonNetwork.Disconnect();
            }
        }
    }
}