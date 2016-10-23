/* EnemySpawner.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Randomly spawns enemies up to a max amount
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class EnemySpawner : MonoBehaviour
    {
        [Tooltip("Prefab of the object to spawn")]
        public GameObject enemyPrefab;

        [Tooltip("Max number of enemies")]
        public int max = 2;

        [Tooltip("Time interval (in seconds) to attempt to spawn a new object")]
        public float interval = 2.0f;

        [Tooltip("Minimum x bound")]
        public float x1 = -50.0f;

        [Tooltip("Maximum x bound")]
        public float x2 = 50.0f;

        [Tooltip("Minimum y bound")]
        public float y1 = -50.0f;

        [Tooltip("Maximum y bound")]
        public float y2 = 50.0f;

        [Tooltip("The radius around the enemy that must be clear in order to spawn")]
        public float spawnClearRadius = 5.0f;

        // Enemy spawn timer
        private float t;

        void Start()
        {
            t = interval;
        }

        void Update()
        {
            // Only master client spawns enemies
            if (!PhotonNetwork.isMasterClient)
                return;

            if (t > 0)
            {
                t -= Time.deltaTime;
                return;
            }
            else
                t = interval;

            // If current number of enemies is less than max, spawn a new enemy
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < max)
            {
                Vector3 spawnPos = new Vector3(Random.Range(x1, x2), Random.Range(y1, y2), 0.0f);

                // Only spawn if the position is clear. Otherwise we will try again next interval
                if (IsPosClear(spawnPos))
                    PhotonNetwork.InstantiateSceneObject(enemyPrefab.name, spawnPos, Quaternion.identity, 0, null);
            }
        }

        // Check if a position is clear for enemy spawning
        private bool IsPosClear(Vector3 position)
        {
            // Create a temporary object with a circle collider to check an area
            GameObject tempObj = new GameObject("Temp Obj");
            tempObj.transform.position = position;
            CircleCollider2D collider = tempObj.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = spawnClearRadius;

            // Cast collider at the position
            RaycastHit2D[] results = new RaycastHit2D[1];
            bool isPosClear = (collider.Cast(Vector2.zero, results) == 0);

            Destroy(tempObj);

            return isPosClear;
        }
    }
}
