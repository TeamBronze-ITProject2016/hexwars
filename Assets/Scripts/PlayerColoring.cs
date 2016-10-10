using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PlayerColoring : Photon.PunBehaviour
    {
        private Color[] colors;

        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            colors = new Color[]
            {
                Color.red,
                Color.yellow,
                Color.green,
                Color.cyan,
                Color.blue,
                Color.magenta
            };

            int seed = GetSeed(photonView.owner.name);
            System.Random random = new System.Random(seed);
            Color playerColor = colors[random.Next(colors.Length - 1)];
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
