using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class GameOverHandler : Photon.PunBehaviour
    {
        GameManager gameManager;

        void Start()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            string collisionObjTag = collision.collider.gameObject.tag;

            if(photonView.isMine && (collisionObjTag == "Triangle" || collisionObjTag ==  "EnemyAttackingPart"))
            {
                PlayerPrefs.SetFloat("finalScore", 0.0f);
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player.ID);
                gameManager.isGameOver = true;
                PhotonNetwork.Disconnect();
            }
        }
    }
}