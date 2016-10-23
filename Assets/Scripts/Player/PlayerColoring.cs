/* PlayerColoring.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Sets the color of player parts using the player's nickname as the seed
 */

using UnityEngine;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PlayerColoring : Photon.PunBehaviour
    {
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // Possible player colors
            Color[] colors = new Color[]
            {
                Color.red,
                Color.yellow,
                Color.green,
                Color.cyan,
                Color.blue,
                Color.magenta
            };

            // Use player name as seed, so the same name will always have the same color (may change between builds)
            int seed = GetSeed(photonView.owner.name);
            System.Random random = new System.Random(seed);
            Color randomColor = colors[random.Next(colors.Length)];

            // Interpolate color with gray (more visually pleasing)
            randomColor = Color.Lerp(randomColor, Color.gray, 0.5f);

            // Set sprite color
            GetComponent<SpriteRenderer>().color = randomColor;
        }

        // Converts a player name to a seed
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
