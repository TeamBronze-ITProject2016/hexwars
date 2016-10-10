using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PlayerColoring : Photon.PunBehaviour
    {
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            int seed = GetSeed(photonView.owner.name);
            System.Random random = new System.Random(seed);
            Color playerColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            GetComponent<SpriteRenderer>().color = playerColor;
        }

        private int GetSeed(string name)
        {
            int seed = 0;

            foreach (char c in name)
            {
                seed += (int)c;
            }

            return seed;
        }
    }
}
