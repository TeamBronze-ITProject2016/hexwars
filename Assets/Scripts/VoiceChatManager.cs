using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class VoiceChatManager : MonoBehaviour
    {
        private PhotonVoiceRecorder voiceRecorder;
        private GameObject localPlayer;
        private GameObject[] players;
        private AudioSource audioSource;

        /*Use this for initialization*/
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            voiceRecorder = GetComponent<PhotonVoiceRecorder>();

            EventManager.registerListener("voiceEnable", startTransmitting);
            EventManager.registerListener("voiceDisable", stopTransmitting);
            EventManager.registerListener("voiceOff", disableVoiceChat);
            EventManager.registerListener("voiceOn", enableVoiceChat);
        }

        /*Enable/disable voice transmission - event callbacks*/
        public void startTransmitting(){
            Debug.Log("voiceEnable()");
            voiceRecorder.Transmit = true;
        }
        public void stopTransmitting(){
            Debug.Log("voiceDisable()");
            voiceRecorder.Transmit = false;
        }

        /*Enable/disable voice chat - event callbacks*/
        public void disableVoiceChat() {
            Debug.Log("Voice chat disabled");
            audioSource.volume = 0.0f;
        }
        public void enableVoiceChat() {
            Debug.Log("Voice chat enabled");
            audioSource.volume = 1.0f;
        }
    }
}