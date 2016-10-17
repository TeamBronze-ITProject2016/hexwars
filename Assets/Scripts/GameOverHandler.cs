using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;

namespace TeamBronze.HexWars
{
    public class GameOverHandler : Photon.PunBehaviour
    {
        GameManager gameManager;
        Scoreboard scoreboard;

        void Start()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<Scoreboard>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            string collisionObjTag = collision.collider.gameObject.tag;

            if(photonView.isMine && (collisionObjTag == "Triangle" || collisionObjTag ==  "EnemyAttackingPart"))
            {
                float highestScore = scoreboard.GetLocalPlayerHighestScore();
                PlayerPrefs.SetFloat("highestScore", highestScore);
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player.ID);
                gameManager.isGameOver = true;
                PhotonNetwork.Disconnect();
            }
        }
    }
}