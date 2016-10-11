using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragToRotate : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    float deltaRotation;
    float previousRotation;
    float currentRotation;
    public float maxRotateSpeed = 50f;
    private bool mouseDown;

    void Update()
    {
        if (mouseDown)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");

            currentRotation = angleBetweenPoints(playerObj.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
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

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("LocalPlayer");
        deltaRotation = 0f;
        previousRotation = angleBetweenPoints(playerObj.transform.position, Camera.main.ScreenToWorldPoint(eventData.position));
        mouseDown = true;
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

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
    }
}
