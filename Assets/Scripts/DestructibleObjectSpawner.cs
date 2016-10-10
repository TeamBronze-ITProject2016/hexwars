using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class DestructibleObjectSpawner : MonoBehaviour
    {
        [Tooltip("Prefab of the object to spawn")]
        public GameObject destructibleObjectPrefab;

        [Tooltip("Max number of destructibles")]
        public int max = 60;

        [Tooltip("Time interval (in seconds) to spawn a new object")]
        public float interval = 2;

        [Tooltip("Minimum x bound")]
        public float x1 = -50.0f;

        [Tooltip("Maximum x bound")]
        public float x2 = 50.0f;

        [Tooltip("Minimum y bound")]
        public float y1 = -50.0f;

        [Tooltip("Maximum y bound")]
        public float y2 = 50.0f;

        public float startingForce = 20.0f;

        public float startingTorque = 5.0f;

        private float t;

        void Start()
        {
            t = interval;
        }

        void Update()
        {
            if (t > 0)
            {
                t -= Time.deltaTime;
                return;
            }
            else
                t = interval;

            if (PhotonNetwork.isMasterClient && GameObject.FindGameObjectsWithTag("DestructibleObject").Length < max)
            {
                GameObject destructibleObj = PhotonNetwork.InstantiateSceneObject(destructibleObjectPrefab.name, GetSpawnPos(), Quaternion.identity, 0, null);
                Rigidbody2D rb = destructibleObj.GetComponent<Rigidbody2D>();
                rb.rotation = Random.Range(0.0f, 360.0f);
                float angleInRad = rb.rotation * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
                rb.AddForce(direction * startingForce);
                rb.AddTorque(startingTorque);
            }
        }

        Vector3 GetSpawnPos()
        {
            /* TODO: Make sure position doesn't collide with anything */
            return new Vector3(Random.Range(x1, x2), Random.Range(y1, y2), 0.0f);
        }
    }
}
