using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class CollisionHandler : Photon.MonoBehaviour
    {
        public string destroyingGameObjectTag;
        public GameObject partAdder;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == destroyingGameObjectTag)
            {
                List<GameObject> list = partAdder.GetComponent<TestNewPartAdder>().findDestroyedObjects(gameObject);
                foreach (GameObject obj in list)
                {
                    PhotonNetwork.Destroy(obj);
                }
            }
        }
    }
}
