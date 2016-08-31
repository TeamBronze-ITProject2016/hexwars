/*ReplayManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Handles storage and playback of replay data
*/
using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    /*Handles GUI drawing.*/
    public class GUIManager : MonoBehaviour {

        /*Classes needed to draw GUI.*/
        private ReplayManager replayManager = null;
        private InputManager inputManager = null;

        /*Replay timer data*/
        private Vector2 replayTimerOffset = new Vector2(1.0f, 1.0f);
        private Color replayTimerColor = Color.red;

        /*Joystick*/
        private Texture joystickInner = null;
        private Texture joystickOuter = null;
        private Vector2 joystickOff = new Vector2(0.0f, 0.0f);
        private const float JOYSTICK_OUTER_RADIUS = 64.0f;
        private const float JOYSTICK_INNER_RADIUS = 48.0f;
        private float joystickMoveRadius = JOYSTICK_OUTER_RADIUS - JOYSTICK_INNER_RADIUS;

        /*Initialise*/
        void Start () {
            /*TODO: fix this - better method?*/
            replayManager = FindObjectOfType<ReplayManager>();
            inputManager = FindObjectOfType<InputManager>();

            if (!replayManager) 
                Debug.LogError("GUIManager::Start() - Could not find replayManager!");

            if (!inputManager) 
                Debug.LogError("GUIManager::Start() - Could not find inputManager!");

            /*Load textures. Should use LoadAll in future.*/
            joystickInner = Resources.Load("JoystickInner") as Texture;
            joystickOuter = Resources.Load("JoystickOuter") as Texture;
        }
        
        /*Draw GUI*/
        void OnGUI () {
            /*Draw replay timer if we are playing a replay*/
            if (replayManager.isPlaying()) {
                float timerTime = replayManager.getPlaybackTime();
                string timerStr = string.Format("Replay: {0}", timerTime);
                GUI.color = replayTimerColor;
                GUI.Label(new Rect(replayTimerOffset.x, replayTimerOffset.y, 128.0f, 32.0f), timerStr);
            }

            /*Draw Joystick*/
            joystickOff = inputManager.lastMoveVector() * joystickMoveRadius;
            Debug.Log(string.Format("joystickOff = {0} {1}", joystickOff.x, joystickOff.y));
            GUI.color = Color.white;
            GUI.DrawTexture(new Rect(0.0f, Screen.height - 
                JOYSTICK_OUTER_RADIUS, JOYSTICK_OUTER_RADIUS, 
                JOYSTICK_OUTER_RADIUS), joystickOuter);
            GUI.DrawTexture(new Rect(JOYSTICK_OUTER_RADIUS/2.0f - JOYSTICK_INNER_RADIUS/2.0f + joystickOff.x, Screen.height - 
                JOYSTICK_OUTER_RADIUS/2.0f - JOYSTICK_INNER_RADIUS/2.0f + joystickOff.y, JOYSTICK_INNER_RADIUS, 
                JOYSTICK_INNER_RADIUS), joystickInner);
        }
    }
}