/* PlayerNameInputField.cs
 * Based on code provided in the PUN Basics Tutorial:
 * https://doc.photonengine.com/en/pun/current/tutorials/pun-basics-tutorial/intro
 * Adapted by: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Manages the player name input field, stores player nickname
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

namespace TeamBronze.HexWars
{
    // Player name input field. Let the user input his name, will appear above the player in the game.
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        // Store the PlayerPref Key to avoid typos
        static string playerNamePrefKey = "PlayerName";

        // MonoBehaviour method called on GameObject by Unity during initialization phase.
        void Start()
        {
            string defaultName = "";
            InputField _inputField = this.GetComponent<InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.playerName = defaultName;
        }

        // Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        public void SetPlayerName(string value)
        {
            // Force a trailing space string in case value is an empty string
            PhotonNetwork.playerName = value + " ";

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
    }
}