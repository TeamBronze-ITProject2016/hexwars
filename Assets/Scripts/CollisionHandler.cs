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
                /*
				PartAdder partAdder = GameObject.FindGameObjectWithTag ("PartAdder").GetComponent<PartAdder> ();
				List<GameObject> list = partAdder.findDestroyedObjects(gameObject);
           
				foreach (GameObject obj in list)
                {
					try{
						foreach (KeyValuePair<int, GameObject> entry in obj.GetComponent<HexagonData>().childDict) {
								if (entry.Value.tag == "Hexagon" || entry.Value.tag == "Player" || entry.Value.tag == "LocalPlayer") {
									entry.Value.GetComponent<HexagonData>().childDict.Remove(partAdder.getOppositePos(entry.Key));
								}
							}
						}catch{
					}
                }

				foreach (GameObject obj in list) {
					try{
						PhotonNetwork.Destroy (obj);
					}catch{
					}
				}
                */
            }
        }
    }
}
