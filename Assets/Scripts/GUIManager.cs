/*GUIManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Handles GUI functions
*/
using UnityEngine;
using System.Collections;

/*TODO: 
 * - Draw player names
 * - Merge all buttons and GUI stuff into this class
 */

namespace TeamBronze.HexWars
{
    /*Handles GUI drawing.*/
    public class GUIManager : MonoBehaviour {

        /*State of the GUI. Controls what GUI elements are drawn.
         TODO: Better names, add more states as needed*/
        enum GUIState {
            InGame = 0,
            InGameMenu = 1
        }

        GUIState state = GUIState.InGame;

        /*Classes needed to draw GUI.*/
        private ReplayManager replayManager = null;
        private InputManager inputManager = null;

        /*Replay timer data*/
        private Vector2 replayTimerOffset = new Vector2(1.0f, 1.0f);
        private Color replayTimerColor = Color.red;

        /*GUI*/
        /*TODO: use ini file, or GameObjects?*/
        private const float GUI_BUTTON_WIDTHFACTOR = 0.09f;
        private const float GUI_BUTTON_HEIGHTFACTOR = 0.04f;

        /*Joystick*/
        private bool joystickEnabled = false;
        private Texture joystickInner = null;
        private Texture joystickOuter = null;
        private Vector2 joystickOff = new Vector2(0.0f, 0.0f);
        private const float JOYSTICK_OUTER_RADIUS = 64.0f;
        private const float JOYSTICK_INNER_RADIUS = 48.0f;
        private float joystickMoveRadius = JOYSTICK_OUTER_RADIUS - JOYSTICK_INNER_RADIUS;

        /*In-game menu*/
        private const float INGAMEMENU_WIDTH_FACTOR = 0.3f;
        private const float INGAMEMENU_HEIGHT_FACTOR = 0.8f;
        private const float INGAMEMENU_CLOSEBTN_WIDTH_FACTOR = 0.2f;
        private const float INGAMEMENU_MENUITEM_WIDTH_FACTOR = 0.85f;
        private const float INGAMEMENU_MENUITEM_HEIGHT_FACTOR = 0.1f;
        private const float INGAMEMENU_MENUITEM_SPACE_FACTOR = 0.05f;

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

            switch (state) {
            /*In-Game*/
            case GUIState.InGame:
                /*Voice chat button*/
                float btnWidth = Screen.width * GUI_BUTTON_WIDTHFACTOR;
                float btnHeight = Screen.width * GUI_BUTTON_HEIGHTFACTOR;

                if(GUI.RepeatButton(new Rect(0.0f, 0.0f, btnWidth, btnHeight), "Voice Chat")){
                    //This is executed as long as the voice chat button is held down.
                    Debug.Log("Voice Chat button held!");
                }

                /*In-Game menu open button*/
                if (GUI.Button(new Rect(Screen.width - btnWidth, 0.0f, btnWidth, btnHeight), "Menu")) {
                    Debug.Log("In-Game menu open button pressed");
                    state = GUIState.InGameMenu;
                }


                /*Draw replay timer if we are playing a replay*/
                if (replayManager.isPlaying()) {
                    float timerTime = replayManager.getPlaybackTime();
                    string timerStr = string.Format("Replay: {0}", timerTime);
                    GUI.color = replayTimerColor;
                    GUI.Label(new Rect(replayTimerOffset.x, replayTimerOffset.y, 128.0f, 32.0f), timerStr);
                }

                /*Draw Joystick*/
                if (joystickEnabled)
                {
                    joystickOff = inputManager.lastMoveVector() * joystickMoveRadius;
                    GUI.color = Color.white;
                    GUI.DrawTexture(new Rect(0.0f, Screen.height -
                        JOYSTICK_OUTER_RADIUS, JOYSTICK_OUTER_RADIUS,
                        JOYSTICK_OUTER_RADIUS), joystickOuter);
                    GUI.DrawTexture(new Rect(JOYSTICK_OUTER_RADIUS / 2.0f - JOYSTICK_INNER_RADIUS / 2.0f + joystickOff.x, Screen.height -
                        JOYSTICK_OUTER_RADIUS / 2.0f - JOYSTICK_INNER_RADIUS / 2.0f + joystickOff.y, JOYSTICK_INNER_RADIUS,
                        JOYSTICK_INNER_RADIUS), joystickInner);
                }
                break;

            /*In-Game Menu*/
            case GUIState.InGameMenu:
                float width = Screen.width*INGAMEMENU_WIDTH_FACTOR;
                float height = Screen.height*INGAMEMENU_HEIGHT_FACTOR;
                float xOff  = (Screen.width - width)/2.0f;
                float yOff = (Screen.height - height)/2.0f;
                GUI.Box(new Rect(xOff, yOff, width, height), "In-Game Menu");

                float menuItemHeight = height*INGAMEMENU_MENUITEM_HEIGHT_FACTOR;
                float menuItemWidth = width*INGAMEMENU_MENUITEM_WIDTH_FACTOR;
                float menuItemOffsetX = (width - menuItemWidth)/2.0f;

                /*Close Button, top right. Sets state to 'in-game' when pressed.*/
                if(GUI.Button(new Rect(xOff + width - width*INGAMEMENU_CLOSEBTN_WIDTH_FACTOR, yOff,
                    width*INGAMEMENU_CLOSEBTN_WIDTH_FACTOR, height*INGAMEMENU_MENUITEM_HEIGHT_FACTOR), "X")){
                    state = GUIState.InGame;
                }

                /*Option 1*/
                yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                if (GUI.Button(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), "Option 1")) {
                    Debug.Log("Option 1 Pressed.");
                }

                /*Option 2*/
                yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                if (GUI.Button(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), "Option 2")) {
                    Debug.Log("Option 2 Pressed.");
                }

                /*Option 3*/
                yOff += height * (INGAMEMENU_MENUITEM_HEIGHT_FACTOR + INGAMEMENU_MENUITEM_SPACE_FACTOR);
                if (GUI.Button(new Rect(xOff + menuItemOffsetX, yOff, menuItemWidth, menuItemHeight), "Option 3")) {
                    Debug.Log("Option 3 Pressed.");
                }
                break;

            default:
                Debug.LogWarning("GUIManager::OnGUI(): Unrecognised state!");
                state = GUIState.InGame;
                break;
            }
        }

        /*Returns true if the pointer is within the given Rect*/
        private bool isPointerInRect(Vector2 pointer, Rect rect) {
            pointer.y = Screen.height - pointer.y;
            //Debug.Log("Pointer: (" + pointer.x + "," + pointer.y + ")");
            //Debug.Log("Rect: (" + rect.xMin + "," + rect.yMin + "), (" + rect.xMax + "," + rect.yMax + ")");
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
            /*Check all the GUI elements visible in each state.*/
            switch (state)
            {
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

        /*Show or hide the joystick*/
        public void setJoystickEnabled(bool val) {
            joystickEnabled = val;
        }
    }
}