/* DestructibleObject.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Manages the behaviour of destructible objects.
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class DestructibleObject : MonoBehaviour
    {
        [Tooltip("The minimum XY pos before despawn")]
        public Vector2 minBound = new Vector2(-50.0f, -50.0f);

        [Tooltip("The maximum XY pos before despawn")]
        public Vector2 maxBound = new Vector2(50.0f, 50.0f);

        void Start()
        {
            // Possible colors for the destructible object
            Color[] colors = new Color[]
            {
                Color.red,
                Color.yellow,
                Color.green,
                Color.cyan,
                Color.blue,
                Color.magenta
            };

            // Get random color and interpolate with grey
            Color randomColor = colors[new System.Random().Next(colors.Length)];
            randomColor = Color.Lerp(randomColor, Color.gray, 0.5f);

            // Set sprite color
            GetComponent<SpriteRenderer>().color = randomColor;
        }

        void Update()
        {
            // Check if out of bounds
            if (transform.position.x < minBound.x ||
                transform.position.y < minBound.y ||
                transform.position.x > maxBound.x ||
                transform.position.y > maxBound.y)
            {
                DestroySelf();
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject collisionObj = collision.collider.gameObject;

            // Check if collision is with a triangle part
            if (collisionObj.tag == "Triangle" || collisionObj.tag == "EnemyAttackingPart")
            {
                // Update points if local player destroyed
                GameObject playerObj = collisionObj.transform.parent.gameObject;
                if (playerObj.tag == "LocalPlayer")
                    playerObj.GetComponent<Player>().points += 1;

                DestroySelf();
            }
        }

        // Destroys the collider component on this destructible object, then fades out
        private void DestroySelf()
        {
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            GetComponent<ObjectFader>().FadeOut();
        }
    }
}