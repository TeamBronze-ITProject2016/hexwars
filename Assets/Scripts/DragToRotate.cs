using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace TeamBronze.HexWars
{
  
    public class DragToRotate : MonoBehaviour, IDragHandler, IPointerDownHandler
    {

        float deltaRotation;
        float previousRotation;
        float currentRotation;
        public float maxRotateSpeed = 50f;

        private GUIManager gui;

        /*Initialise*/
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

        /*Returns false if the GUI is in a state where no joystick input should be accepted,
        * or true otherwise.*/
        private bool active() {
        if (gui.gameOverSplashVisible() || gui.inGameMenuVisible())
            return false;
                
           return true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!active())
                return;

            GameObject playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");
            deltaRotation = 0f;
            previousRotation = angleBetweenPoints(playerObj.transform.position, Camera.main.ScreenToWorldPoint(eventData.position));
        }

        float angleBetweenPoints(Vector2 position1, Vector2 position2)
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
            if (!active())
                return;

            GameObject playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");

            currentRotation = angleBetweenPoints(playerObj.transform.position, Camera.main.ScreenToWorldPoint(eventData.position));
                deltaRotation = Mathf.DeltaAngle(currentRotation, previousRotation);
                if (deltaRotation > maxRotateSpeed)
                {
                    deltaRotation = maxRotateSpeed;
                }
                if (deltaRotation < -maxRotateSpeed)
                {
                    deltaRotation = -maxRotateSpeed;
                }
                previousRotation = currentRotation;

                playerObj.transform.Rotate(Vector3.back, deltaRotation);
        }
    }
}
