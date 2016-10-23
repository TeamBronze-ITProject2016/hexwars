/* DestructibleObjectSpawner.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Spawns destructible objects within bounds up to a max amount.
 */

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

        [Tooltip("Minimum XY pos to spawn at")]
        public Vector2 minBound = new Vector2(-50.0f, -50.0f);

        [Tooltip("Maximum XY pos to spawn at")]
        public Vector2 maxBound = new Vector2(50.0f, 50.0f);

        [Tooltip("The magnitude of the force that a destructible object spawns with")]
        public float startingForce = 20.0f;

        [Tooltip("The magnitude of the torque that a destructible object spawns with")]
        public float startingTorque = 5.0f;

        void Update()
        {
            // Check if num of destructible objects is less than max
            if (GameObject.FindGameObjectsWithTag("DestructibleObject").Length < max)
            {
                // Spawn new destructible object
                GameObject destructibleObj = (GameObject)Instantiate(destructibleObjectPrefab, GetSpawnPos(), Quaternion.identity);

                // Initialize rotation/velocity/torque of destructible object
                Rigidbody2D rb = destructibleObj.GetComponent<Rigidbody2D>();
                rb.rotation = Random.Range(0.0f, 360.0f);
                float angleInRad = rb.rotation * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
                rb.AddForce(direction * startingForce);
                rb.AddTorque(RandomNegOrPos() * startingTorque);

                // Remove if spawned on top of something
                if (destructibleObj.GetComponent<PolygonCollider2D>().Cast(Vector2.zero, new RaycastHit2D[1], 0) > 0)
                    Destroy(destructibleObj);
            }
        }

        // Gets a random spawn position within the min and max bounds
        private Vector3 GetSpawnPos()
        {
            return new Vector3(Random.Range(minBound.x, maxBound.x), Random.Range(minBound.y, maxBound.y), 0.0f);
        }

        // Coin flip, returns either -1 or 1
        private int RandomNegOrPos()
        {
            if (Random.value > 0.5)
                return -1;
            else
                return 1;
        }
    }
}
