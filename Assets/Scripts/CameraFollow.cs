using UnityEngine;
using System.Collections;


namespace TeamBronze.HexWars
{
    public class CameraFollow : MonoBehaviour
    {
        Transform cameraTransform;

        // Use this for initialization
        void Start()
        {
            /* TODO: Check if attached to local player */
            if(true) {
                cameraTransform = Camera.main.transform;
            }
        }

        void LateUpdate()
        {
            /* Set camera position to this GameObject's position */
            cameraTransform.position = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);
        }
    }
}

