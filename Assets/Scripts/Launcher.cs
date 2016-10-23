/* Launcher.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Sets up / allows us to connect to PUN.
 * 
 * Based on code provided in the PUN Basics Tutorial
 * https://doc.photonengine.com/en/pun/current/tutorials/pun-basics-tutorial/intro
 */

using UnityEngine;

namespace TeamBronze.HexWars
{
    public class Launcher : Photon.PunBehaviour
    {
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        public GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        public GameObject progressLabel;

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        public byte MaxPlayersPerRoom = 20;

        // This client's version number
        string _gameVersion = "1.0";

        private bool isConnecting;

        // MonoBehaviour method called on GameObject by Unity during early initialization phase.
        void Awake()
        {
            // Force Full LogLevel
            PhotonNetwork.logLevel = Loglevel;

            // We don't join the lobby. There is no need to join a lobby to get the list of rooms.
            PhotonNetwork.autoJoinLobby = false;

            // This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients
            // in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;
        }

        // MonoBehaviour method called on GameObject by Unity during initialization phase.
        void Start()
        {
            // Hide "Connecting..." text
            progressLabel.SetActive(false);

            // Display main menu GUI
            controlPanel.SetActive(true);
        }

        // Start the connection process. If already connected, we attempt joining a random room
        public void Connect()
        {
            isConnecting = true;

            // Display "Connecting..." text
            progressLabel.SetActive(true);

            // Hide menu GUI
            controlPanel.SetActive(false);

            // We check if we are connected or not, we join if we are, otherwise we initiate the connection to the server.
            if (PhotonNetwork.connected)
            {
                // Try to join a room. If it fails, we'll be notified and we can create a new room
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // Connect to photon server
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
            }
        }

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
                PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");

            // Hide "Connecting..." text
            progressLabel.SetActive(false);

            // Show menu GUI
            controlPanel.SetActive(true);
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

            // We failed to join a random room, so try creating a new room
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            // We only load if we are the first player, else we rely on PhotonNetwork.automaticallySyncScene to sync our instance scene.
            if (PhotonNetwork.room.playerCount == 1)
            {
                // Load the Room Level
                PhotonNetwork.LoadLevel("Room");
            }
        }
    }
}
