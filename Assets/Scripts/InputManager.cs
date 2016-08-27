using UnityEngine;
using System.Collections;


namespace TeamBronze.HexWars
{
    public class InputManager : MonoBehaviour
    {
        [Tooltip("0 = mouse, 1 = ...")]
        public int inputType = 0;

        // Use this for initialization
        void Start()
        {

        }

        bool IsActive()
        {
            /* Mouse */
            if (inputType == 0)
            {
                return Input.GetMouseButton(0);
            }
            else
            {
                throw new System.Exception("Invalid inputType value");
            }
        }

        Vector2 GetPos()
        {
            /* Mouse */
            if (inputType == 0)
            {
                return (Vector2)Input.mousePosition;
            }
            else
            {
                throw new System.Exception("Invalid inputType value");
            }
        }
    }
}
