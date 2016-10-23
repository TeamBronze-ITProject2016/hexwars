/* CameraFollow.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Makes the camera appropriately follow the local player.
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class CameraFollow : Photon.PunBehaviour
    {
        [Tooltip("Base level of zoom for the camera")]
        public float zoomBase = 5;
        
        [Tooltip("Ratio to zoom out by when the player gets larger")]
        public float zoomRatio = 1;

        // Reference to the main camera
        private Camera mainCamera;

        void Start()
        {
            Debug.Log("CameraFollow: Initialising....");

            // Make sure that this script is only active for the local player
            if(photonView.isMine) {
                mainCamera = Camera.main;
            }
        }

        void LateUpdate()
        {
            // Return if no reference to main camera
            if (!mainCamera) { return; }

            SetPos(mainCamera);
            SetZoom(mainCamera);
        }

        // Sets the position of the camera to the position of the player
        private void SetPos(Camera camera)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        }

        // Sets the zoom of the camera based on the current mass of the player and the given public variables
        private void SetZoom(Camera camera)
        {
            float mass = GetComponent<Rigidbody2D>().mass;
            mainCamera.orthographicSize = zoomBase + zoomRatio * mass;
        }
    }
}
