using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace TeamBronze.HexWars {

    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

        private Image backgroundImage;
        private Image joystickImage;
        private Vector3 inputVector;

        private GUIManager gui;

        private void Start()
        {
            // Make it so area fits half of screen
            GameObject parent = transform.parent.gameObject;
            GetComponent<Image>().rectTransform.sizeDelta = new Vector3(parent.GetComponent<RectTransform>().rect.width / 2,
                                                                         parent.GetComponent<RectTransform>().rect.height,
                                                                         0);
            gui = FindObjectOfType<GUIManager>();
            Debug.Assert(gui);
            backgroundImage = transform.FindChild("JoystickBackground").GetComponent<Image>();
            joystickImage = transform.FindChild("JoystickBackground").transform.FindChild("JoystickImage").GetComponent<Image>();
            backgroundImage.enabled = false;
            joystickImage.enabled = false;
        }

        /*Returns false if the GUI is in a state where no joystick input should be accepted,
         * or true otherwise.*/
        private bool active() {
            if (gui.gameOverSplashVisible() || gui.inGameMenuVisible())
                return false;

            return true;
        }

        public virtual void OnDrag(PointerEventData ped)
        {
            if (!active())
                return;

            Vector2 pos = ped.position;

            /*Don't enable joystick if we're over a GUI element.*/
            if(gui.isPointerOverGUIElement(new Vector2(ped.position.x, ped.position.y)))
                return;

            // Get position ratio from 0,1
            pos.x = ((pos.x - backgroundImage.transform.position.x) / backgroundImage.rectTransform.sizeDelta.x);
            pos.y = ((pos.y - backgroundImage.transform.position.y) / backgroundImage.rectTransform.sizeDelta.y);
            // Normalize positions so that 0,0 is centre of joystick
            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            // Normalize if position of x,y is greater than 1
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move joystick image to touch position
            joystickImage.rectTransform.anchoredPosition = new Vector3(
                inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 2),
                inputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 2),
                0);
        }

        public virtual void OnPointerDown(PointerEventData ped)
        {
            /*Don't draw joystick if not active or over a gui element*/
            if (!active() || gui.isPointerOverGUIElement(new Vector2(ped.position.x, ped.position.y)))
                return;

            // Enable joystick and move to correct position
            backgroundImage.enabled = true;
            joystickImage.enabled = true;

            Vector2 newPos = new Vector2(ped.position.x + backgroundImage.rectTransform.sizeDelta.x / 2,
                                         ped.position.y - backgroundImage.rectTransform.sizeDelta.y / 2);

            backgroundImage.transform.position = newPos;
            joystickImage.transform.position = newPos;

            // Start drag component
            OnDrag(ped);
        }

        public virtual void OnPointerUp(PointerEventData ped)
        {
            // Reset joystick when let go
            inputVector = Vector3.zero;
            //joystickImage.rectTransform.anchoredPosition = Vector3.zero;

            backgroundImage.enabled = false;
            joystickImage.enabled = false;
        }

        public float getHorizontal()
        {
            return inputVector.x;
        }

        public float getVertical()
        {
            return inputVector.z;
        }
    }
}
