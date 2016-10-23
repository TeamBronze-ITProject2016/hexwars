/* GameManager.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Handles loading the game room and spawning players
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_5_3
using UnityEngine.Experimental.Networking;
#else
using UnityEngine.Networking;
#endif

namespace TeamBronze.HexWars
{
    public class GameManager : Photon.PunBehaviour
    {
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        [Tooltip("The radius around the enemy that must be clear in order to spawn")]
        public float spawnClearRadius = 5.0f;

        [Tooltip("Minimum x bound")]
        public float x1 = -50.0f;

        [Tooltip("Maximum x bound")]
        public float x2 = 50.0f;

        [Tooltip("Minimum y bound")]
        public float y1 = -50.0f;

        [Tooltip("Maximum y bound")]
        public float y2 = 50.0f;

        [HideInInspector]
        public bool isGameOver = false;

        static public GameManager Instance;

        private ReplayManager replayManager;

        // Initialize
        void Start()
        {
            EventManager.registerListener("playagain", SpawnLocalPlayer);

            if (playerPrefab == null)
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            else
                SpawnLocalPlayer();

            replayManager = FindObjectOfType<ReplayManager>();
            Debug.Assert(replayManager);

            EventManager.registerListener("disconnect", LeaveRoom);
            EventManager.registerListener("replayStart", DisableSending);
            EventManager.registerListener("playagain", DoReconnect);
        }

        // Called when the local player leaves the room
        public override void OnLeftRoom()
        {
            if (isGameOver)
                SceneManager.LoadScene(2);
            else
                SceneManager.LoadScene(0);
        }

        // Leave the current room
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        // Create an instance of the local player at a random location free of other players.
        void SpawnLocalPlayer()
        {
            // Keep trying to spawn local player until we find a clear position
            Vector3 spawnPos;
            do
            {
                spawnPos = new Vector3(UnityEngine.Random.Range(x1, x2), UnityEngine.Random.Range(y1, y2), 0.0f);
            }
            while (!IsPosClear(spawnPos));

            GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos, Quaternion.identity, 0);
        }

        // Check if a position is clear of other players
        bool IsPosClear(Vector3 position)
        {
            GameObject tempObj = new GameObject("Temp Obj");
            tempObj.transform.position = position;
            CircleCollider2D collider = tempObj.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = spawnClearRadius;
            RaycastHit2D[] results = new RaycastHit2D[1];
            bool isPosClear = (collider.Cast(Vector2.zero, results) == 0);
            Destroy(tempObj);

            return isPosClear;
        }
        
        // Replay callbacks
        void DisableSending()
        {
            PhotonNetwork.SetSendingEnabled(0, false);
            PhotonNetwork.SetReceivingEnabled(0, false);
        }
        void DoReconnect()
        {
            PhotonNetwork.SetSendingEnabled(0, true);
            PhotonNetwork.SetReceivingEnabled(0, true);
            LeaveRoom();
            PhotonNetwork.JoinRandomRoom();
        }
    }
}
