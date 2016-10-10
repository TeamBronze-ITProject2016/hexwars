using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class LocalDestructibleObjectSpawner : MonoBehaviour
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

        [Tooltip("The magnitude of the force that a destructible object spawns with")]
        public float startingForce = 20.0f;

        [Tooltip("The magnitude of the torque that a destructible object spawns with")]
        public float startingTorque = 5.0f;

        private float t;

        void Start()
        {
            t = interval;
        }

        void Update()
        {
            if (GameObject.FindGameObjectsWithTag("DestructibleObject").Length < max)
            {
                GameObject destructibleObj = (GameObject)Instantiate(destructibleObjectPrefab, GetSpawnPos(), Quaternion.identity);
                Rigidbody2D rb = destructibleObj.GetComponent<Rigidbody2D>();
                rb.rotation = Random.Range(0.0f, 360.0f);
                float angleInRad = rb.rotation * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
                rb.AddForce(direction * startingForce);
                rb.AddTorque(RandomNegOrPos() * startingTorque);

                if (destructibleObj.GetComponent<PolygonCollider2D>().Cast(Vector2.zero, new RaycastHit2D[1], 0) > 0)
                    Destroy(destructibleObj);
            }
        }

        Vector3 GetSpawnPos()
        {
            /* TODO: Make sure position doesn't collide with anything */
            return new Vector3(Random.Range(x1, x2), Random.Range(y1, y2), 0.0f);
        }

        int RandomNegOrPos()
        {
            if (Random.value > 0.5)
                return -1;
            else
                return 1;
        }
    }
}
