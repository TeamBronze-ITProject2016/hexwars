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

        /* Called when the local player left the room. We need to load the launcher scene. */
        public override void OnLeftRoom()
        {
            if (isGameOver)
                SceneManager.LoadScene(2);
            else
                SceneManager.LoadScene(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.name); /* Not seen if you're the player connecting */

            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); /* Called before OnPhotonPlayerDisconnected */
                //LoadArena();
            }
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.name); /* Seen when other disconnects */

            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); /* Called before OnPhotonPlayerDisconnected */
                //LoadArena();
                // Remove player from server

                /* ----- OLD -----
                string url = "http://128.199.229.64/remove/";
                string name = PhotonNetwork.player.name;
                UnityWebRequest request = UnityWebRequest.Get(url + name);
                request.Send();
                */
            }
            // Update the scoreboard
            GameObject scoreboard = GameObject.FindGameObjectWithTag("ScoreBoard");
            PhotonView scoreboardView = PhotonView.Get(scoreboard);
            scoreboardView.RPC("UpdateScoresBoard", PhotonTargets.All);
        }

        void Start()
        {
            EventManager.registerListener("playagain", spawnLocalPlayer);

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                spawnLocalPlayer();
            }

            replayManager = FindObjectOfType<ReplayManager>();
            Debug.Assert(replayManager);

            EventManager.registerListener("disconnect", LeaveRoom);
            EventManager.registerListener("replayStart", DisableSending);
            EventManager.registerListener("playagain", DoReconnect);
        }

        /*Create an instance of the local player at a random location free of other players.*/
        void spawnLocalPlayer() {
            Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);

            /* We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate */
            Vector3 spawnPos;
            do
            {
                spawnPos = new Vector3(UnityEngine.Random.Range(x1, x2), UnityEngine.Random.Range(y1, y2), 0.0f);
            }
            while (!IsPosClear(spawnPos));

            GameObject playerObj = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPos, Quaternion.identity, 0);
        }

        void LoadArena()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level : Room");
            PhotonNetwork.LoadLevel("Room");
        }

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
        
        /*Replay callbacks.*/
        void DisableSending() {
            PhotonNetwork.SetSendingEnabled(0, false);
            PhotonNetwork.SetReceivingEnabled(0, false);
        }
        void DoReconnect() {
            PhotonNetwork.SetSendingEnabled(0, true);
            PhotonNetwork.SetReceivingEnabled(0, true);
            LeaveRoom();
            PhotonNetwork.JoinRandomRoom();
        }
    }
}
