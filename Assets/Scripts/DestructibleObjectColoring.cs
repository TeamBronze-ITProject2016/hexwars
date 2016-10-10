using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class DestructibleObjectColoring : Photon.PunBehaviour
    {
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
        }
    }
}
