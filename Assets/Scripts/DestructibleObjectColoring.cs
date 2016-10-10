using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class DestructibleObjectColoring : Photon.PunBehaviour
    {
        public Color baseColor = new Color(0.5f, 0.5f, 0.5f);
        public float colorStrength = 0.5f;

        private Color[] colors;

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

            GetComponent<SpriteRenderer>().color = colors[new System.Random().Next(colors.Length)];
        }
    }
}
