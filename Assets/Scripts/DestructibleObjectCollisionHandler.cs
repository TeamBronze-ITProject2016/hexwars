using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class DestructibleObjectCollisionHandler : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            /* TODO: Add points to player */
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
