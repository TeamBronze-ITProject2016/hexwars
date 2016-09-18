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
				TestNewPartAdder partAdder = GameObject.FindGameObjectWithTag ("PartAdder").GetComponent<TestNewPartAdder> ();
				List<GameObject> list = partAdder.findDestroyedObjects(gameObject);;
           
				foreach (GameObject obj in list)
                {
					foreach (KeyValuePair<int, GameObject> entry in obj.GetComponent<HexagonData>().childDict) {
						if (entry.Value.tag == "Hexagon") {
							entry.Value.GetComponent<HexagonData> ().childDict.Remove (partAdder.getOppositePos(entry.Key));
						}
					}
                }
				foreach (GameObject obj in list) {
					PhotonNetwork.Destroy (obj);
				}
            }
        }
    }
}
