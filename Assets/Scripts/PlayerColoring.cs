using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PlayerColoring : Photon.PunBehaviour
    {
        private Color[] colors;

        public Color baseColor = new Color(0.5f, 0.5f, 0.5f);
        public float colorStrength = 0.5f;

        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            colors = new Color[]
            {
                Color.Lerp(baseColor, Color.red, colorStrength),
                Color.Lerp(baseColor, Color.yellow, colorStrength),
                Color.Lerp(baseColor, Color.green, colorStrength),
                Color.Lerp(baseColor, Color.cyan, colorStrength),
                Color.Lerp(baseColor, Color.blue, colorStrength),
                Color.Lerp(baseColor, Color.magenta, colorStrength),
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
