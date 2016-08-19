using UnityEngine;

namespace Com.TeamBronze.Hexwars {
	public class Launcher : Photon.PunBehaviour {
		#region Public Variables

		/// <summary>
		/// The PUN loglevel.
		/// </summary>
		public PhotonLogLevel Loglevel = PhotonLogLevel.Full;


		/// <summary>
		/// The maximum number of players per room. When a room is full, it can't
		/// be joined by new players, and so a new room will be created.
		/// </summary>
		[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so a new room will be created")]
		public byte MaxPlayersPerRoom = 10;

		[Tooltip("The UI Panel to let the user enter name, connect and play")]
		public GameObject controlPanel;
		[Tooltip("The UI Label to inform the user that the connection is in progress")]
		public GameObject progressLabel;
		[Tooltip("The UI Label to inform the user that the connection was successfully made")]
		public GameObject completedLabel;

		#endregion

		#region Private Variables

		/// <summary>
		/// This client's version number. Users are separated from each other by
		/// gameversion (which allows you to make breaking changes).
		/// </summary>
		string _gameVersion = "1";

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early
		/// initialization phase.
		/// </summary>
		void Awake() {

			// #NotImportant
			// Force LogLevel
			PhotonNetwork.logLevel = Loglevel;

			// #Critical
			// we don't join the lobby. There is no need to join a lobby to get the
			// list of rooms. We don't need the Lobby features!
			PhotonNetwork.autoJoinLobby = false;

			// #Critical// this makes sure we can use PhotonNetwork.LoadLevel() on
			// the master client and all clients in the same room sync their level
			// automatically
			PhotonNetwork.automaticallySyncScene = true;
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during
		/// initialization phase.
		/// </summary>
		void Start() {
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			completedLabel.SetActive(false);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Start the connection process.
		/// - If already connected, we attempt joining a random room
		/// - If not yet connected, Connect this application instance to
		/// 	Photon Cloud Network
		/// </summary>
		public void Connect() {

			progressLabel.SetActive(true);
			controlPanel.SetActive(false);
			completedLabel.SetActive(false);

			// we check if we are connected or not, we join if we are, else we
			// initiate the connection to the server.
			if (PhotonNetwork.connected) {
				// #Critical we need at this point to attempt joining a Random Room.
				// If it fails, we'll get notified in OnPhotonRandomJoinFailed() and
				// we'll create one.
				PhotonNetwork.JoinRandomRoom();
			} else {
				// #Critical we must first and foremost connect to Photon Online Server
				// Starting point for the game to connect to Photon Cloud
				PhotonNetwork.ConnectUsingSettings(_gameVersion);
			}
		}

		#endregion

		#region Photon.PunBehaviour CallBacks

		public override void OnConnectedToMaster() {
			progressLabel.SetActive(false);
			controlPanel.SetActive(false);
			completedLabel.SetActive(true);

			Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");

			// #Critical The first thing we try to do is to join a potential exising
			// room. If there is, good, else, we'll be called back with
			// OnPhotonRandomJoinFailed()
			PhotonNetwork.JoinRandomRoom();
		}

		public override void OnDisconnectedFromPhoton() {
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			completedLabel.SetActive(false);

			Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
		}

		public override void OnPhotonRandomJoinFailed (object[] codeAndMsg) {
			Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room availale, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = MaxPlayersPerRoom}, null);");

			// #Critical we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = MaxPlayersPerRoom }, null);
		}

		public override void OnJoinedRoom() {
			Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
		}

		#endregion

	}
}
