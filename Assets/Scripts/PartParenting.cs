/* PartParenting.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Sets part parents to the appropriate player, so that their positions are synchronized properly.
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PartParenting : Photon.PunBehaviour
    {
        // Called on instantiation by PUN
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // Get ID of who instantiated the obj
            int ownerID = info.sender.ID;

            // Get all non-local players
            GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");

            // Find player with the correct ID and set as parent
            for(int i = 0; i < playerObjs.Length; i++)
            {
                if(playerObjs[i].GetPhotonView().owner.ID == ownerID)
                    gameObject.transform.SetParent(playerObjs[i].transform);
            }
        }
    }
}