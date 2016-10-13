using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace TeamBronze.HexWars
{
    public class GameManager : Photon.PunBehaviour
    {
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        [Tooltip("Minimum x bound")]
        public float x1 = -50.0f;

        [Tooltip("Maximum x bound")]
        public float x2 = 50.0f;

        [Tooltip("Minimum y bound")]
        public float y1 = -50.0f;

        [Tooltip("Maximum y bound")]
        public float y2 = 50.0f;

        static public GameManager Instance;

        private ReplayManager replayManager;

        /* Called when the local player left the room. We need to load the launcher scene. */
        public override void OnLeftRoom()
        {
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
            }
        }

        void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                /* We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate */
                Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(x1, x2), UnityEngine.Random.Range(y1, y2), 0.0f);

                PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPos, Quaternion.identity, 0);
            }

            replayManager = FindObjectOfType<ReplayManager>();
            Debug.Assert(replayManager);

            EventManager.registerListener("disconnect", LeaveRoom);
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
    }
}
