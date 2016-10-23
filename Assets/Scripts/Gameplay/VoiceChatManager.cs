/* VoiceChatManager.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Manages Photon Voice settings
 */

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

        // Initialize
        void Start()
        {
            // Get components used by Photon Voice
            audioSource = GetComponent<AudioSource>();
            voiceRecorder = GetComponent<PhotonVoiceRecorder>();

            EventManager.registerListener("voiceEnable", startTransmitting);
            EventManager.registerListener("voiceDisable", stopTransmitting);
            EventManager.registerListener("voiceOff", disableVoiceChat);
            EventManager.registerListener("voiceOn", enableVoiceChat);
        }

        // Enable voice transmission - event callbacks
        public void startTransmitting()
        {
            Debug.Log("voiceEnable()");
            voiceRecorder.Transmit = true;
        }

        // Disable voice transmission - event callbacks
        public void stopTransmitting()
        {
            Debug.Log("voiceDisable()");
            voiceRecorder.Transmit = false;
        }

        // Disable voice chat - event callback
        public void disableVoiceChat()
        {
            Debug.Log("Voice chat disabled");
            audioSource.volume = 0.0f;
        }

        // Enable voice chat - event callback
        public void enableVoiceChat()
        {
            Debug.Log("Voice chat enabled");
            audioSource.volume = 1.0f;
        }
    }
}