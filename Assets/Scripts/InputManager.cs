/*InputManager.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: Handles user input.
*/
using UnityEngine;
using System.Collections;

/*TODO:
 * -When touch controls are used, make sure that the touch is in the bounds of the inner
 * joystick.*/

namespace TeamBronze.HexWars
{
    /*Handles all the input functionality*/
    public class InputManager : MonoBehaviour
    {
        /*Input type*/
        public enum InputType {
            Mouse = 0,
            Touch = 1,
        }

        [Tooltip("0 = mouse, 1 = touch")]
        public InputType inputType = InputType.Mouse;

        /*Used for drawing joystick.*/
        private Vector2 lastMove;

        public bool IsActive(){
            lastMove = new Vector2(0.0f, 0.0f);

            /* Mouse */
            if (inputType == InputType.Mouse)
                return Input.GetMouseButton(0);

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

        /*Returns a unit vector indicating the direction of movement. Used to draw the
         * joystick.*/
        public Vector2 lastMoveVector() {
            /*Return null vector if there is no movement input.*/
            if (lastMove == new Vector2(0.0f, 0.0f))
                return new Vector2(0.0f, 0.0f);

            /*Calculate movement vector*/
            Vector2 unit = new Vector2(Screen.width, Screen.height) / 2.0f - lastMove;
            unit.Normalize();
            unit.x *= -1.0f;
            return unit;
        }
    }
}
