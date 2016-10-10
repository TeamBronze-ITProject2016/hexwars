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
        private PhotonVoiceSpeaker voiceSpeaker;

        private GameObject localPlayer;
        private GameObject[] players;

        // Use this for initialization
        void Start(){
            voiceRecorder = GetComponent<PhotonVoiceRecorder>();
            voiceSpeaker = GetComponent<PhotonVoiceSpeaker>();
            voiceRecorder.Transmit = enabledByDefault;

            EventManager.registerListener("voiceEnable", voiceEnable);
            EventManager.registerListener("voiceDisable", voiceDisable);
        }

        // Update is called once per frame
        void Update()
        {
            return;

            if (localPlayer == null)
            {
                localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");

                if (localPlayer == null)
                    return;
                else
                {
                    PhotonVoiceNetwork.Client.GlobalAudioGroup = (byte)localPlayer.GetPhotonView().owner.ID;
                    PhotonVoiceNetwork.Client.ChangeAudioGroups(new byte[0], null);
                }
            }

            players = GameObject.FindGameObjectsWithTag("Player");

            for(int i = 0; i < players.Length; i++) {
                if(Vector3.Distance(localPlayer.transform.position, players[i].transform.position) < chatRadius)
                {
                    byte[] temp = new byte[1];
                    temp[0] = (byte)players[i].GetPhotonView().owner.ID;

                    PhotonVoiceNetwork.Client.ChangeAudioGroups(null, temp);
                }
                else
                {
                    byte[] temp = new byte[1];
                    temp[0] = (byte)players[i].GetPhotonView().owner.ID;

                    PhotonVoiceNetwork.Client.ChangeAudioGroups(temp, null);
                }
            }
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