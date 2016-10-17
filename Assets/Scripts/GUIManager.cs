/*GUIManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Handles GUI functions
*/
using UnityEngine;
using System.Collections;

/*Uses the Blop sound effect from http://soundbible.com/2067-Blop.html
Created by Mark DiAngelo. Used under the Creative Commons Attribution 3.0 
License: https://creativecommons.org/licenses/by/3.0/au/legalcode */

namespace TeamBronze.HexWars {
    /*Handles GUI drawing.*/
    public class GUIManager : MonoBehaviour {

        /*State of the GUI. Controls what GUI elements are drawn.
         TODO: Better names, add more states as needed*/
        enum GUIState {
            InGame = 0,
            InGameMenu = 1,
            GameOver = 2,
        }

        GUIState state = GUIState.InGame;

        /*Audio*/
        private AudioSource GUIAudioSource;
        private AudioClip elementSelectedClip;
        private float guiAudioVolume = 1.0f;

        /*Voice chat*/
        private bool voiceLast = false;
        private bool voiceOn = true;
        private bool voiceOnLast = true;
        private float voiceToggleTime = -1.0f;
        private const float VOICE_TOGGLE_DELAY = 0.25f;

        /*Images*/
        private Texture2D voiceicondisabled;
        private Texture2D voiceiconenabled;
        private Texture2D gameoverText;

        /*Classes needed to draw GUI.*/
        private InputManager inputManager;

        /*Replay timer data*/
        private Vector2 replayTimerOffset = new Vector2(1.0f, 1.0f);
        private Color replayTimerColor = Color.red;

        /*GUI*/
        private const float GUI_BUTTON_WIDTHFACTOR = 0.09f;
        private const float GUI_BUTTON_HEIGHTFACTOR = 0.04f;

        /*Joystick*/
        //private bool joystickEnabled = false;
        //private Texture joystickInner = null;
        //private Texture joystickOuter = null;
        //private Vector2 joystickOff = new Vector2(0.0f, 0.0f);
        //private const float JOYSTICK_OUTER_RADIUS = 64.0f;
        //private const float JOYSTICK_INNER_RADIUS = 48.0f;
        //private float joystickMoveRadius = JOYSTICK_OUTER_RADIUS - JOYSTICK_INNER_RADIUS;

        /*In-game menu*/
        private const float INGAMEMENU_WIDTH_FACTOR = 0.3f;
        private const float INGAMEMENU_HEIGHT_FACTOR = 0.8f;
        private const float INGAMEMENU_CLOSEBTN_WIDTH_FACTOR = 0.2f;
        private const float INGAMEMENU_MENUITEM_WIDTH_FACTOR = 0.85f;
        private const float INGAMEMENU_MENUITEM_HEIGHT_FACTOR = 0.1f;
        private const float INGAMEMENU_MENUITEM_SPACE_FACTOR = 0.05f;

        /*Game over screen*/
        private const float GAMEOVER_WIDTH_FACTOR = 0.45f;
        private const float GAMEOVER_HEIGHT_FACTOR = 0.15f;
        private const float GAMEOVER_Y_OFFSETFACTOR = 0.35f;
        private const float GAMEOVER_PLAYAGAIN_WIDTH_FACTOR = GUI_BUTTON_WIDTHFACTOR;
        private const float GAMEOVER_PLAYAGAIN_HEIGHT_FACTOR = GUI_BUTTON_HEIGHTFACTOR;

        /*Initialise*/
        void Start() {
            /*TODO: fix this - better method?*/
            inputManager = FindObjectOfType<InputManager>();

            if (!inputManager)
                Debug.LogError("GUIManager::Start() - Could not find inputManager!");

            /*Register for game over event*/
            EventManager.registerListener("gameover", onGameOver);
                
            /*Load textures. Should use LoadAll in future.*/
            //joystickInner = Resources.Load("JoystickInner") as Texture;
            //joystickOuter = Resources.Load("JoystickOuter") as Texture;
            voiceicondisabled = (Texture2D)Resources.Load("voiceicondisabled");
            voiceiconenabled = (Texture2D)Resources.Load("voiceiconenabled");
            gameoverText = (Texture2D)Resources.Load("gameover");
            //joystickInner = Resources.Load("JoystickInner") as Texture;
            //joystickOuter = Resources.Load("JoystickOuter") as Texture;

            /*Initialise audio*/
            GUIAudioSource = GetComponent<AudioSource>();
            elementSelectedClip = (AudioClip)Resources.Load("Audio/Blop-Mark_DiAngelo-79054334");
            Debug.Assert(GUIAudioSource);
            Debug.Assert(elementSelectedClip);
        }

        /*Draw GUI*/
        void OnGUI() {

            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;

            switch (state) {
                /*In-Game*/
                case GUIState.InGame:
                    /*Voice chat button and events*/
                    float btnWidth = Screen.width * GUI_BUTTON_WIDTHFACTOR;
                    float btnHeight = Screen.width * GUI_BUTTON_HEIGHTFACTOR;

                    Texture2D voiceIcon = voiceLast ? voiceiconenabled : voiceicondisabled;

                    if (voiceOn && GUI.RepeatButton(new Rect(0.0f, 2.0f, btnWidth, btnHeight), voiceIcon, GUIStyle.none)) {
                        voiceToggleTime = Time.time;

                        if (!voiceLast) {
                            voiceLast = true;
                            EventManager.triggerEvent("voiceEnable");
                        }
                    } else if (voiceLast && Time.time - voiceToggleTime > VOICE_TOGGLE_DELAY){
                        voiceLast = false;
                        EventManager.triggerEvent("voiceDisable");
                    }

                    // HIDDEN BECAUSE INCOMPLETE
                    /*In-Game menu open button*/
                    /*
                    if (GUI.Button(new Rect(Screen.width - btnWidth, 0.0f, btnWidth, btnHeight), "Menu")) {
                        Debug.Log("In-Game menu open button pressed");
                        state = GUIState.InGameMenu;
                        onElementSelected();
                    }*/

                    /*Draw replay timer if we are playing a replay*/
                    if (ReplayManager.isPlaying()) {
                        float timerTime = ReplayManager.getPlaybackTime();
                        string timerStr = string.Format("Replay: {0}", timerTime);
                        GUI.color = replayTimerColor;
                        GUI.Label(new Rect(replayTimerOffset.x, replayTimerOffset.y, 128.0f, 32.0f), timerStr);
                    }

                    /*Draw Joystick - replaced by VirtualJoystick
                    if (joystickEnabled) {
                        joystickOff = inputManager.lastMoveVectorUnit() * joystickMoveRadius;
                        GUI.color = Color.white;
                        GUI.DrawTexture(new Rect(0.0f, Screen.height -
                            JOYSTICK_OUTER_RADIUS, JOYSTICK_OUTER_RADIUS,
                            JOYSTICK_OUTER_RADIUS), joystickOuter);
                        GUI.DrawTexture(new Rect(JOYSTICK_OUTER_RADIUS / 2.0f - JOYSTICK_INNER_RADIUS / 2.0f + joystickOff.x, Screen.height -
                            JOYSTICK_OUTER_RADIUS / 2.0f - JOYSTICK_INNER_RADIUS / 2.0f + joystickOff.y, JOYSTICK_INNER_RADIUS,
                            JOYSTICK_INNER_RADIUS), joystickInner);
                    }*/
                    break;

                /*In-Game Menu*/
                case GUIState.InGameMenu:
                    float width = Screen.width * INGAMEMENU_WIDTH_FACTOR;
                    float height = Screen.height * INGAMEMENU_HEIGHT_FACTOR;
                    float xOff = (Screen.width - width) / 2.0f;
                    float yOff = (Screen.height - height) / 2.0f;
                    GUI.Box(new Rect(xOff, yOff, width, height), "In-Game Menu");

                    float menuItemHeight = height * INGAMEMENU_MENUITEM_HEIGHT_FACTOR;
                    float menuItemWidth = width * INGAMEMENU_MENUITEM_WIDTH_FACTOR;
                    float menuItemOffsetX = (width - menuItemWidth) / 2.0f;

                    /*Close Button, top right. Sets state to 'in-game' when pressed.*/
                    if (GUI.Button(new Rect(xOff + width - width * INGAMEMENU_CLOSEBTN_WIDTH_FACTOR, yOff,
                        width * INGAMEMENU_CLOSEBTN_WIDTH_FACTOR, height * INGAMEMENU_MENUITEM_HEIGHT_FACTOR), "X")) {
                        state = GUIState.InGame;
                        onElementSelected();
                    }

                    /*Option 1*/
                    yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                    voiceOn = GUI.Toggle(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), voiceOn, "Voice Chat Enabled");

                    if(voiceOn != voiceOnLast && Time.time - voiceToggleTime > VOICE_TOGGLE_DELAY){

                        voiceOnLast = voiceOn;
                        voiceToggleTime = Time.time;

                        if (voiceOn) {
                            EventManager.triggerEvent("voiceOn");
                        } else {
                            EventManager.triggerEvent("voiceOff");
                        }

                        onElementSelected();
                    }

                    /*Option 2
                    yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                    if (GUI.Button(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), "Option 2")) {
                        Debug.Log("Option 2 Pressed.");
                        onElementSelected();
                    }

                    yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                    if (GUI.Button(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), "Option 3")) {
                        Debug.Log("Option 3 Pressed.");
                        onElementSelected();
                    }

                    yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                    if (GUI.Button(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), "Option 4")) {
                        Debug.Log("Option 4 Pressed.");
                        onElementSelected();
                    }

                    yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                    if (GUI.Button(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), "Disconnect")) {
                        EventManager.triggerEvent("disconnect");
                        onElementSelected();
                    }*/
                    break;

                /*Draw game over splash screen*/
                case GUIState.GameOver:
                    if (ReplayManager.isPlaying())
                        break;

                    /*Game over text*/
                    float gameoverWidth = GAMEOVER_WIDTH_FACTOR * Screen.width;
                    float gameoverHeight = GAMEOVER_HEIGHT_FACTOR * Screen.height;
                    float gameoverXOff = (Screen.height - gameoverWidth);///2.0f;
                    float gameoverYOff = GAMEOVER_Y_OFFSETFACTOR * Screen.height;
                    GUI.DrawTexture(new Rect(gameoverXOff, gameoverYOff, gameoverWidth, gameoverHeight), gameoverText);

                    /*Play again button - TODO: Fix this
                   if (GUI.Button(new Rect(gameoverXOff, gameoverYOff + gameoverHeight,
                        GAMEOVER_PLAYAGAIN_WIDTH_FACTOR * Screen.width, GAMEOVER_PLAYAGAIN_HEIGHT_FACTOR * Screen.height), "Play Again")) {
                            EventManager.triggerEvent("playagain");
                            state = GUIState.InGame;
                            onElementSelected();
                    }*/

                    /*Disconnect button*/
                    if (GUI.Button(new Rect(gameoverXOff + GAMEOVER_PLAYAGAIN_WIDTH_FACTOR*Screen.width + 4, gameoverYOff + gameoverHeight, 
                        GAMEOVER_PLAYAGAIN_WIDTH_FACTOR * Screen.width, GAMEOVER_PLAYAGAIN_HEIGHT_FACTOR * Screen.height), "Disconnect")) {
                            EventManager.triggerEvent("disconnect");
                            onElementSelected();
                    }

                    break;

                default:
                    Debug.LogWarning("GUIManager::OnGUI(): Unrecognised state!");
                    state = GUIState.InGame;
                    break;
            }
        }

        void Update() {
            ReplayManager.doUpdate();
        }

        /*Should be called after any element has been selected.*/
        private void onElementSelected() {
            Debug.Log("Playing element selected sound effect");
            GUIAudioSource.PlayOneShot(elementSelectedClip, guiAudioVolume);
        }

        /*Returns true if the pointer is within the given Rect*/
        private bool isPointerInRect(Vector2 pointer, Rect rect) {
            pointer.y = Screen.height - pointer.y;

            /*X bounds*/
            if (pointer.x < rect.xMin || pointer.x > rect.xMax)
                return false;

            /*Y bounds*/
            if (pointer.y < rect.yMin || pointer.y > rect.yMax)
                return false;

            return true;
        }

        /*Returns true if the pointer is over the voice chat button.*/
        private bool isPointerOverVoiceBtn(Vector2 pointer) {
            float btnWidth = Screen.width * GUI_BUTTON_WIDTHFACTOR;
            float btnHeight = Screen.width * GUI_BUTTON_HEIGHTFACTOR;
            return isPointerInRect(pointer, new Rect(0.0f, 0.0f, btnWidth, btnHeight));
        }

        /*Returns true if the pointer is over the in-game menu open button.*/
        private bool isPointerOverMenuBtn(Vector2 pointer) {
            float btnWidth = Screen.width * GUI_BUTTON_WIDTHFACTOR;
            float btnHeight = Screen.width * GUI_BUTTON_HEIGHTFACTOR;
            return isPointerInRect(pointer, new Rect(Screen.width - btnWidth, 0.0f, btnWidth, btnHeight));
        }

        public bool isPointerOverGUIElement(Vector2 pointer) {
            //Debug.Log("pointer: (" + pointer.x + "," + pointer.y + ")");
            /*Check all the GUI elements visible in each state.*/
            switch (state) {
                case GUIState.InGame:
                    if (isPointerOverMenuBtn(pointer) || isPointerOverVoiceBtn(pointer))
                        return true;

                    break;

                default:
                    break;
            }

            return false;
        }

        /*Return true if the in-game menu is visible.*/
        public bool isInGameMenuShown() {
            return (state == GUIState.InGameMenu);
        }

        /*Show or hide the joystick
        /*public void setJoystickEnabled(bool val) {
            joystickEnabled = val;
        }*/

        /*Game over event*/
        private void onGameOver() {
            this.state = GUIState.GameOver;
        }

        /*Returns true if the game over screen is visible*/
        public bool gameOverSplashVisible() {
            return (state == GUIState.GameOver);
        }

        /*Returns true if the in-game menu is visible*/
        public bool inGameMenuVisible() {
            return (state == GUIState.InGameMenu);
        }
    }
}