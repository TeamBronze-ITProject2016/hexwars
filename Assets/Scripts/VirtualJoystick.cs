using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image backgroundImage;
    private Image joystickImage;
    private Vector3 inputVector;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        // Check if joystick is touched
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform,
                                                                    ped.position,
                                                                    ped.pressEventCamera,
                                                                    out pos))
        {
            // Get position ratio from 0,1
            pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);
            // Normalize positions so that 0,0 is centre of joystick
            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            // Normalize if position of x,y is greater than 1
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move joystick image to touch position
            joystickImage.rectTransform.anchoredPosition = new Vector3(
                inputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 2),
                inputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 2),
                0);

            //Debug.Log("mov" + inputVector.x);
            //Debug.Log("mov" + inputVector.y);
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        // Reset joystick when let go
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
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
