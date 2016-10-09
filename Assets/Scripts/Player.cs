/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TeamBronze.HexWars
{
    public class Player : Photon.PunBehaviour
    {
        [Tooltip("Required rotation in order for the player to actually begin rotating")]
		public float rotationThreshold;

        [Tooltip("Forward acceleration of the player")]
        public float acceleration;

        [Tooltip("Rotation speed of the player (constant rate)")]
        public float rotationSpeed;

        [Tooltip("The minimum x and y positions that the player is allowed to be at")]
        public Vector2 minBound = new Vector2(-50.0f, -50.0f);

        [Tooltip("The maximum x and y positions that the player is allowed to be at")]
        public Vector2 maxBound = new Vector2(50.0f, 50.0f);

        [HideInInspector]
        public Rigidbody2D rb;

        [HideInInspector]
        public int points = 0;

        private InputManager inputManager; /* Need to fix namespaces */

        public ArrayList partList = new ArrayList();

        /*Initialise*/
        void Start()
        {
			if (photonView.isMine)
            {
				gameObject.tag = "LocalPlayer";
			}

            rb = GetComponent<Rigidbody2D>();
            inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
            partList.Add(this.gameObject);
            ReplayManager.registerGameObject(this.gameObject);
        }

        /*Called once per frame*/
        void FixedUpdate()
        {
            if (!inputManager)
            {
                Debug.Log("Player - Warning: Not initialised!");
                Start();
            }

            if (!photonView.isMine)
            {
                return;
            }

            if (inputManager.IsActive())
            {
                Vector2 coordinate = inputManager.GetPos();
                RotateToPoint(coordinate);
                MoveForward();
            }

            KeepInBoundary();
        } 

        /* Apply a forward force to the hexagon. */
        void MoveForward()
        {
            float angleInRad = rb.rotation * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
            rb.AddForce(direction * acceleration * rb.mass);
        }

        /* Rotate hexagon at a constant rate towards a certain point */
        void RotateToPoint(Vector2 coord)
        {
            /* Find the vector pointing from our position to the target */
            Vector3 p = rb.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(coord.x, coord.y, 1));

			float targetAngle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;

			float rightRotateAmount = normalizeAngle(normalizeAngle(targetAngle) - normalizeAngle(transform.rotation.eulerAngles.z));

			int rotateDir;
			// Don't rotate if within minimum threshold
			if (rightRotateAmount < rotationThreshold || rightRotateAmount > 360 - rotationThreshold)
				rotateDir = 0;
			// Rotate Left
			else if (rightRotateAmount > 180f)
				rotateDir = -1;
			// Rotate Right
			else
				rotateDir = 1;

			transform.RotateAround(transform.position, Vector3.forward, rotateDir * rotationSpeed * Time.deltaTime);
        }

        void KeepInBoundary()
        {
            if (transform.position.x < minBound.x)
                transform.position = new Vector2(minBound.x, transform.position.y);

            if (transform.position.y < minBound.y)
                transform.position = new Vector2(transform.position.x, minBound.y);

            if (transform.position.x > maxBound.x)
                transform.position = new Vector2(maxBound.x, transform.position.y);

            if (transform.position.y > maxBound.y)
                transform.position = new Vector2(transform.position.x, maxBound.y);
        }

		// Normalizes angle to 0 degrees to 360 degrees
		private float normalizeAngle(float angle){
			angle = angle % 360;
			if (angle < 0) {
				angle += 360;
			}
			return angle;
		}
    }
}