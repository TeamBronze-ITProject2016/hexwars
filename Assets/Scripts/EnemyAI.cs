/* EnemyAI.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Controls the movement of enemies.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class EnemyAI : Photon.PunBehaviour
    {
        [Tooltip("Forward acceleration amount")]
        public float acceleration = 50.0f;

        [Tooltip("Rotation speed")]
        public float rotationSpeed = 250.0f;

        [Tooltip("The minimum amount of time before switching modes")]
        public float changeModeIntervalMin = 1.0f;

        [Tooltip("The minimum amount of time before switching modes")]
        public float changeModeIntervalMax = 3.0f;

        [Tooltip("Chance that the switched-to mode will be the chase mode")]
        public float chaseProbability = 0.5f;

        [Tooltip("Maximum range to lock onto an object to chase")]
        public float maxChaseRange = 20.0f;

        [Tooltip("Minimum XY pos")]
        public Vector2 minBound = new Vector2(-50.0f, -50.0f);

        [Tooltip("Maximum XY pos")]
        public Vector2 maxBound = new Vector2(50.0f, 50.0f);

        // Reference to the enemy's Rigidbody2d
        private Rigidbody2D rb;

        // Current mode, either "move" or "chase"
        // Move mode picks a random direction and moves in that direction
        // Chase mode finds the nearest attackable obj within maxChaseRange and moves towards that object
        private string mode = "move";

        // Time until mode switch
        private float t;

        // Object to chase (in chase mode)
        private GameObject objToChase;

        // Direction to move (in move mode)
        private float directionToMove;

        // Called when this object is instantiated with PUN
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            rb = GetComponent<Rigidbody2D>();

            // Only master client controls movement of AI enemies
            if (!PhotonNetwork.isMasterClient)
                return;

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            ResetTime();
        }

        void Update()
        {
            // Only master client controls movement of AI enemies
            if (!PhotonNetwork.isMasterClient)
                return;

            MovementManagement();
            KeepInBoundary();

            // Check if it's time to switch modes
            if (t > 0.0f)
            {
                t -= Time.deltaTime;
                return;
            }

            SwitchMode();
            ResetTime();
        }

        // Switches to a random mode based on chaseProbability, either "move" or "chase"
        private void SwitchMode()
        {
            if (Random.value < chaseProbability)
            {
                mode = "chase";
                objToChase = FindNearestAttackableObj();
            }

            else
            {
                mode = "move";
                directionToMove = Random.Range(0.0f, 360.0f);
            }

        }

        // Moves depending on which mode we are currently in
        private void MovementManagement()
        {
            if (mode == "chase")
            {
                if (objToChase == null)
                    SwitchMode();
                else
                    MoveTowards(objToChase.transform.position);
            }
            else
            {
                float radians = directionToMove * Mathf.Deg2Rad;
                MoveTowards(transform.position + new Vector3(-(float)Mathf.Cos(radians), -(float)Mathf.Sin(radians), 0.0f));
            }
        }

        // Finds the nearest hexagon/player gameobject (returns null if nothing in range)
        private GameObject FindNearestAttackableObj()
        {
            List<GameObject> gameObjList = new List<GameObject>();

            gameObjList.AddRange(GameObject.FindGameObjectsWithTag("Hexagon"));
            gameObjList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
            gameObjList.AddRange(GameObject.FindGameObjectsWithTag("LocalPlayer"));

            GameObject curObj;
            float curDist;
            GameObject nearestObj = null;
            float nearestDist = maxChaseRange + 1.0f;

            for (int i = 0; i < gameObjList.Count; i++)
            {
                curObj = gameObjList[i];
                curDist = Vector3.Distance(transform.position, curObj.transform.position);

                if (curDist < nearestDist && curDist < maxChaseRange)
                {
                    nearestObj = curObj;
                    nearestDist = curDist;
                }
            }

            return nearestObj;
        }

        // Move towards a position by rotating towards it while moving forward
        private void MoveTowards(Vector3 position)
        {
            RotateToPoint(position);
            MoveForward();
        }

        // Move forward
        private void MoveForward()
        {
            float angleInRad = rb.rotation * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
            rb.AddForce(direction * acceleration * rb.mass);
        }
        
        // Rotate towards a point at a constant rate
        private void RotateToPoint(Vector3 coord)
        {
            Vector3 p = new Vector3(rb.position.x, rb.position.y, 1) - new Vector3(coord.x, coord.y, 1);

            float targetAngle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime));
        }

        // Keep inside the specified boundary. If at a boundary, switch mode to move and the direction away
        // from the boundary (so that we don't get stuck at a boundary)
        private void KeepInBoundary()
        {
            if (transform.position.x < minBound.x)
            {
                transform.position = new Vector3(minBound.x, transform.position.y, transform.position.z);
                mode = "move";
                directionToMove = 180.0f;
            }

            if (transform.position.y < minBound.y)
            {
                transform.position = new Vector3(transform.position.x, minBound.y, transform.position.z);
                mode = "move";
                directionToMove = 270.0f;
            }

            if (transform.position.x > maxBound.x)
            {
                transform.position = new Vector3(maxBound.x, transform.position.y, transform.position.z);
                mode = "move";
                directionToMove = 0.0f;
            }

            if (transform.position.y > maxBound.y)
            {
                transform.position = new Vector3(transform.position.x, maxBound.y, transform.position.z);
                mode = "move";
                directionToMove = 90.0f;
            }
        }

        // Reset mode switch timer to a random amount between the min and max
        private void ResetTime()
        {
            t = Random.Range(changeModeIntervalMin, changeModeIntervalMax);
        }
    }
}
