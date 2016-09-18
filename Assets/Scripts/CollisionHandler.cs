using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBronze.HexWars
{
    public class CollisionHandler : Photon.MonoBehaviour
    {
        public string destroyingGameObjectTag;

        void OnCollisionEnter2D(Collision2D collision)
        {
			Debug.Log (collision.gameObject.tag);
			if (collision.collider.gameObject.tag == "Triangle")
            {
				List<GameObject> list = GameObject.FindGameObjectWithTag("PartAdder").GetComponent<TestNewPartAdder>().findDestroyedObjects(gameObject);;
           
				foreach (GameObject obj in list)
                {
                    PhotonNetwork.Destroy(obj);
                }
            }
        }
    }
}
