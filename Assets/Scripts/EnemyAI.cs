using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class EnemyAI : Photon.PunBehaviour
    {
        public float acceleration = 50.0f;
        public float rotationSpeed = 250.0f;
        public float changeModeIntervalMin = 1.0f;
        public float changeModeIntervalMax = 3.0f;
        public float chaseProbability = 0.5f;
        public float maxChaseRange = 20.0f;
        public float minX = -50.0f;
        public float maxX = 50.0f;
        public float minY = -50.0f;
        public float maxY = 50.0f;

        private Rigidbody2D rb;
        private string mode = "move";
        private float t = 4.0f;
        private GameObject objToChase;
        private float directionToMove;

        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            rb = GetComponent<Rigidbody2D>();

            if (!PhotonNetwork.isMasterClient)
                return;

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            ResetTime();
        }

        void Update()
        {
            if (!PhotonNetwork.isMasterClient)
                return;

            MovementManagement();
            KeepInBoundary();

            if (t > 0.0f)
            {
                t -= Time.deltaTime;
                return;
            }

            SwitchMode();
            ResetTime();
        }

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

        private void MoveTowards(Vector3 position)
        {
            RotateToPoint(position);
            MoveForward();
        }

        private void MoveForward()
        {
            float angleInRad = rb.rotation * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
            rb.AddForce(direction * acceleration * rb.mass);
        }

        private void RotateToPoint(Vector3 coord)
        {
            Vector3 p = new Vector3(rb.position.x, rb.position.y, 1) - new Vector3(coord.x, coord.y, 1);

            float targetAngle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime));
        }

        private void KeepInBoundary()
        {
            if (transform.position.x < minX)
            {
                transform.position = new Vector3(minX, transform.position.y, transform.position.z);
                mode = "move";
                directionToMove = 180.0f;
            }

            if (transform.position.y < minY)
            {
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);
                mode = "move";
                directionToMove = 270.0f;
            }

            if (transform.position.x > maxX)
            {
                transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
                mode = "move";
                directionToMove = 0.0f;
            }

            if (transform.position.y > maxY)
            {
                transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
                mode = "move";
                directionToMove = 90.0f;
            }
        }

        private void ResetTime()
        {
            t = Random.Range(changeModeIntervalMin, changeModeIntervalMax);
        }
    }
}