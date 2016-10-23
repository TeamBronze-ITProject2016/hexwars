/* InputManager.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Handles user input.
 * Note: CURRENTLY UNUSED. This was used for the old movement method.
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    /* Handles all the input functionality */
    public class InputManager : MonoBehaviour
    {
        /* Input type */
        public enum InputType
        {
            Mouse = 0,
            Touch = 1,
        }

        [Tooltip("0 = mouse, 1 = touch")]
        public InputType inputType = InputType.Mouse;

        /*Used for drawing joystick.*/
        private Vector2 lastMove;

        /*GUI Manager*/
        GUIManager GUI = null;

        bool inReplay = false;

        /*Initialise*/
        void Start(){
            /*Find GUIManager*/
            GUI = FindObjectOfType<GUIManager>();
            Debug.Assert(GUI != null);

            EventManager.registerListener("replayStart", replayStart);
            EventManager.registerListener("replayStop", replayStop);
        }

        public bool IsActive(){
            lastMove = new Vector2(0.0f, 0.0f);

            if (inReplay)
                return false;

            /* Mouse */
            if (inputType == InputType.Mouse) {
                /*May need to remove this*/
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return false;

                if (!Input.GetMouseButton(0))
                    return false;

                /*Return false if in-game menu is visible.*/
                if (GUI.isInGameMenuShown())
                    return false;

                /*Check if pointer is above any of the GUI elements*/
                Vector2 pos = GetPos();

                if (GUI.isPointerOverGUIElement(pos))
                    return false;

                return true;
            }

            /* Touch */
            if (inputType == InputType.Touch)
                return Input.touchCount > 0;

            /*Unrecognised input type.*/
            throw new System.Exception("InputManager::IsActive() - Invalid inputType value!");
        }

        public Vector2 GetPos(){
            /* Mouse */
            if (inputType == InputType.Mouse){
                lastMove = Input.mousePosition;
                return (Vector2)Input.mousePosition;
            }

            /*Touch*/
            if (inputType == InputType.Touch) {
                if (Input.touchCount > 0){
                    lastMove = Input.GetTouch(0).position;
                    return Input.GetTouch(0).position;
                }else{
                    throw new System.Exception(@"InputManager::GetPos() - Tried to 
                                                GetPos() with no touch!");
                }
            }

            /*Unrecognised input type.*/
            throw new System.Exception("InputManager::GetPos() - Invalid inputType value!");
        }

        /* Returns a vector indicating the direction of movement */
        public Vector2 lastMoveVector()
        {
            /*Return null vector if there is no movement input.*/
            if (lastMove == new Vector2(0.0f, 0.0f))
                return new Vector2(0.0f, 0.0f);

            /*Calculate movement vector*/
            Vector2 unit = new Vector2(Screen.width, Screen.height) / 2.0f - lastMove;
            return unit;
        }

        /*Returns a unit vector indicating the direction of movement.*/
        public Vector2 lastMoveVectorUnit() {
            Vector2 unit = lastMoveVector();
            unit.Normalize();
            unit.x *= -1.0f;
            return unit;
        }

        /*Replay start/stop event callbacks*/
        public void replayStart() {
            inReplay = true;
        }
        public void replayStop() {
            inReplay = false;
        }
    }
}
