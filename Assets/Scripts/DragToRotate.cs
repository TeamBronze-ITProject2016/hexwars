/* DragToRotate.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Controls the player drag-to-rotate mechanic
 */

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace TeamBronze.HexWars
{
    public class DragToRotate : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("Maximum drag-to-rotate speed")]
        public float maxRotateSpeed = 5.0f;

        private float deltaRotation;
        private float previousRotation;
        private float currentRotation;
        private bool touch = false;

        private GUIManager gui;

        void Start()
        {
            // Make it so area fits half of screen
            GameObject parent = transform.parent.gameObject;
            GetComponent<Image>().rectTransform.sizeDelta = new Vector3(parent.GetComponent<RectTransform>().rect.width/2,
                                                                         parent.GetComponent<RectTransform>().rect.height,
                                                                         0);
            gui = FindObjectOfType<GUIManager>();
                Debug.Assert(gui);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Active())
                return;

            GameObject playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");
            deltaRotation = 0f;
            previousRotation = AngleBetweenPoints(playerObj.transform.position, Camera.main.ScreenToWorldPoint(eventData.position));
        }

        // Returns false if the GUI is in a state where no joystick input should be accepted, or true otherwise.
        private bool Active()
        {
            if (gui.gameOverSplashVisible() || gui.inGameMenuVisible())
                return false;

            return true;
        }

        // Returns the angle between two points
        private float AngleBetweenPoints(Vector2 position1, Vector2 position2)
        {
            var fromLine = position2 - position1;
            var toLine = new Vector2(1, 0);

            var angle = Vector2.Angle(fromLine, toLine);
            var cross = Vector3.Cross(fromLine, toLine);

            // did we wrap around?
            if (cross.z > 0)
                angle = 360f - angle;

            return angle;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Active())
                return;

            GameObject playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");

            currentRotation = AngleBetweenPoints(playerObj.transform.position, Camera.main.ScreenToWorldPoint(eventData.position));
                deltaRotation = Mathf.DeltaAngle(currentRotation, previousRotation);
                if (deltaRotation > maxRotateSpeed)
                    deltaRotation = maxRotateSpeed;

                if (deltaRotation < -maxRotateSpeed)
                    deltaRotation = -maxRotateSpeed;

                previousRotation = currentRotation;

                playerObj.transform.Rotate(Vector3.back, deltaRotation);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Nothing
        }
    }
}
