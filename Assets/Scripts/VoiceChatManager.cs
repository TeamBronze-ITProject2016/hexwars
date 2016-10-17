using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class VoiceChatManager : MonoBehaviour
    {
        [Tooltip("How many frames to wait before updating audio groups")]
        public int waitFrames;

        [Tooltip("Distance at which players can communicate")]
        public float chatRadius;

        public bool enabledByDefault = false;

        private PhotonVoiceRecorder voiceRecorder;
        private GameObject localPlayer;
        private GameObject[] players;

        // Use this for initialization
        void Start()
        {
            voiceRecorder = GetComponent<PhotonVoiceRecorder>();
            voiceRecorder.Transmit = enabledByDefault;

            EventManager.registerListener("voiceEnable", voiceEnable);
            EventManager.registerListener("voiceDisable", voiceDisable);
        }

        public void voiceEnable()
        {
            Debug.Log("voiceEnable()");
            voiceRecorder.Transmit = true;
        }

        public void voiceDisable()
        {
            Debug.Log("voiceDisable()");
            voiceRecorder.Transmit = false;
        }
    }
}