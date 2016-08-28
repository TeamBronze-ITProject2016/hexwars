using UnityEngine;
using System.Collections;


namespace TeamBronze.HexWars
{
    public class CameraFollow : MonoBehaviour
    {
        public float zoomBase = 5;
        public float zoomRatio = 1;
        Camera mainCamera;

        // Use this for initialization
        void Start()
        {
            /* TODO: Check if attached to local player */
            if(true) {
                mainCamera = Camera.main;
            }
        }

        void LateUpdate()
        {
            setPos(mainCamera);
            setZoom(mainCamera);
        }

        void setPos(Camera camera)
        {
            mainCamera.transform.position = new Vector3(
                transform.position.x, transform.position.y, mainCamera.transform.position.z
            );
        }

        void setZoom(Camera camera)
        {
            float mass = GetComponent<Rigidbody2D>().mass;
            mainCamera.orthographicSize = zoomBase + zoomRatio * mass;
        }
    }
}

