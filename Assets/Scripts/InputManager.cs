using UnityEngine;
using System.Collections;


namespace TeamBronze.HexWars
{
    public class InputManager : MonoBehaviour
    {
        [Tooltip("0 = mouse, 1 = touch")]
        public int inputType = 0;

        // Use this for initialization
        void Start()
        {

        }

        public bool IsActive()
        {
            /* Mouse */
            if (inputType == 0)
            {
                return Input.GetMouseButton(0);
            }
            else if (inputType == 1)
            {
                return Input.touchCount > 0;
            }
            else
            {
                throw new System.Exception("Invalid inputType value");
            }
        }

        public Vector2 GetPos()
        {
            /* Mouse */
            if (inputType == 0)
            {
                return (Vector2)Input.mousePosition;
            }
            else if (inputType == 1) {
                if (Input.touchCount > 0)
                {
                    return Input.GetTouch(0).position;
                }
                else
                {
                    throw new System.Exception("Tried to GetPos() with no touch");
                }
            }
            else
            {
                throw new System.Exception("Invalid inputType value");
            }
        }
    }
}
