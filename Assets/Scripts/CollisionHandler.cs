using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class CollisionHandler : MonoBehaviour
    {
        public string destroyingGameObjectTag;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == destroyingGameObjectTag)
                Destroy(gameObject);
        }
    }
}
