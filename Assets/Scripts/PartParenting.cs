using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PartParenting : Photon.PunBehaviour
    {
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            int ownerID = info.sender.ID;

            GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");

            for(int i = 0; i < playerObjs.Length; i++)
            {
                if(playerObjs[i].GetPhotonView().owner.ID == ownerID)
                    gameObject.transform.SetParent(playerObjs[i].transform);
            }
        }
    }
}