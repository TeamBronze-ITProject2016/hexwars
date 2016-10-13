using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class LocalDestructibleObjectBoundary : Photon.MonoBehaviour
    {
        public Vector2 minBound = new Vector2(-50.0f, -50.0f);
        public Vector2 maxBound = new Vector2(50.0f, 50.0f);

        void Update()
        {
            if(    transform.position.x < minBound.x
                || transform.position.y < minBound.y
                || transform.position.x > maxBound.x
                || transform.position.y > maxBound.y)
            {
                GetComponent<LocalObjectFader>().FadeOut();
            }
        }
    }
}