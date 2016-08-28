using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace TeamBronze.HexWars
{
    public class GameManager : MonoBehaviour
    {
        /* Called when the local player left the room. We need to load the launcher scene. */
        public void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}