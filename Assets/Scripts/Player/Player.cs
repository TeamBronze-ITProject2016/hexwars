/* Player.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Handles basic player movement, including player boundaries.
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
        
        [Tooltip("The player's current points (visible in inspector for testing purposes)")]
        public int points = 0;

        // Initialize
        void Start()
        {
            // Assign different tag for the local player
			if (photonView.isMine)
				gameObject.tag = "LocalPlayer";

            rb = GetComponent<Rigidbody2D>();
            ReplayManager.registerGameObject(this.gameObject);
        }

        // Called every fixed framerate frame
        void FixedUpdate()
        {
            // Only move local player
            if (!photonView.isMine)
                return;

            JoyStickMove();
            KeepInBoundary();
        } 

        // Move using the joystick
        private bool JoyStickMove()
        {
            VirtualJoystick joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<VirtualJoystick>();

            Vector3 direction = Vector3.zero;

            direction.x = joystick.getHorizontal();
            direction.y = joystick.getVertical();

            if (direction.x == 0 && direction.y == 0)
                return false;

            rb.AddForce(direction * acceleration * rb.mass);

            return true;
        }

        // Clamp the player's position inside the specified XY boundary
        private void KeepInBoundary()
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

        // Apply a forward force to the hexagon.
        // Note: Currently un-used, part of an old movement system and has been replaced by joystick.
        private void MoveForward()
        {
            // Get normalized direction vector from current player rotation
            float angleInRad = rb.rotation * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));

            // Multiply added force by current mass so that players of all sizes move at the same speed
            rb.AddForce(direction * acceleration * rb.mass);
        }

        // Rotate hexagon at a constant rate towards a certain point
        // Note: Currently un-used, part of an old movement system and has been replaced by joystick.
        private void RotateToPoint(Vector2 coord)
        {
            /* Find the vector pointing from our position to the target */
            Vector3 p = rb.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(coord.x, coord.y, 1));

            float targetAngle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;

            float rightRotateAmount = NormalizeAngle(NormalizeAngle(targetAngle) - NormalizeAngle(transform.rotation.eulerAngles.z));

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

        // Normalizes angle to 0 degrees to 360 degrees
        // Note: Currently un-used, part of an old movement system and has been replaced by joystick.
        private float NormalizeAngle(float angle)
        {
            angle = angle % 360;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }
    }
}