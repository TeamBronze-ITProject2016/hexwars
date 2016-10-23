/* PointsText.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Displays the local player's current points
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TeamBronze.HexWars
{
    public class PointsText : MonoBehaviour
    {
        private GameObject localPlayer;
        private Text text;

        void Start()
        {
            text = GetComponent<Text>();
        }

        void Update()
        {
            // If we haven't found the local player yet, try again
            if(localPlayer == null)
            {
                localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
                return;
            }

            // Update text to match local player's points
            text.text = localPlayer.GetComponent<Player>().points.ToString();
        }
    }
}