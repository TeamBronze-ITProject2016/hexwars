using UnityEngine;
using System.Collections;


namespace TeamBronze.HexWars
{
    public class CameraFollow : Photon.PunBehaviour
    {
        public float zoomBase = 5;
        public float zoomRatio = 1;

        Camera mainCamera;

        // Use this for initialization
        void Start()
        {
            Debug.Log("CameraFollow: Initialising....");

            /* TODO: Check if attached to local player */
            if(photonView.isMine) {
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
            if (!mainCamera){
                Debug.Log("CameraFollow: mainCamera does not exist!");
                return;
            }

            mainCamera.transform.position = new Vector3(
                transform.position.x, transform.position.y, mainCamera.transform.position.z
            );
        }

        void setZoom(Camera camera)
        {
            if (!mainCamera) {
                Debug.Log("CameraFollow: mainCamera does not exist!");
                return;
            }

            float mass = GetComponent<Rigidbody2D>().mass;
            mainCamera.orthographicSize = zoomBase + zoomRatio * mass;
        }
    }
}
