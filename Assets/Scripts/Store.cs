/* Store.cs
 * Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
 * Description: Controls the store where players can spend points to add parts.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace TeamBronze.HexWars
{
    public class Store : MonoBehaviour
    {
        [Tooltip("Default triangle sprite")]
        public Sprite triangleSprite;

        [Tooltip("Sprite for when a triangle cannot be added")]
        public Sprite noTriangleSprite;

        [Tooltip("Reference to the store game object")]
        public GameObject store;

        [Tooltip("Minimum number of points for the store to be displayed")]
        public int storeMinimum = 3;

        private PartAdder partAdder;
        private InputManager inputManager;
        private GameObject player;

        private bool isTriangleUnavailable;
        private float t = 1.0f;

        void Start()
        {
            StartCoroutine(LateStart());
            store.SetActive(false);
        }
        
        // Delay store's start so that other objects are initialized
        IEnumerator LateStart()
        {
            yield return new WaitForSeconds(3);
            inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
            partAdder = GameObject.FindGameObjectWithTag("PartAdder").GetComponent<PartAdder>();
            player = GameObject.FindGameObjectWithTag("LocalPlayer");
        }

        // Update is called once per frame
        void Update()
        {
            // Display score if player has enough points
            if (player != null && (player.GetComponent<Player>().points >= storeMinimum && !store.GetActive()))
            {
                store.SetActive(true);
                GameObject.FindGameObjectWithTag("TriangleStoreIcon").GetComponent<Image>().sprite = triangleSprite;

                if (!inputManager.IsActive()) 
                    return;

                Vector2 pos = -inputManager.lastMoveVector();
            }

            // If triangle unavailable icon is shown, check if we need to switch the icon back yet
            if (isTriangleUnavailable)
            {
                if (t > 0.0f)
                {
                    t -= Time.deltaTime;
                }
                else
                {
                    isTriangleUnavailable = false;
                    GameObject.FindGameObjectWithTag("TriangleStoreIcon").GetComponent<Image>().sprite = triangleSprite;
                }
            }
        }

        // Add hexagon to the player
        public void addHexagon()
        {
            if(partAdder.addRandomPart("Hexagon"))
            {
                player.GetComponent<Player>().points -= storeMinimum;
                store.SetActive(false);
            }

        }

        // Add triangle to the player
        public void addTriangle()
        {
            if (partAdder.addRandomPart("Triangle"))
            {
                player.GetComponent<Player>().points -= storeMinimum;
                store.SetActive(false);
            }
            // If we fail to add a triangle temporarily switch the store icon to indicate that a triangle cannot be added
            else
            {
                GameObject.FindGameObjectWithTag("TriangleStoreIcon").GetComponent<Image>().sprite = noTriangleSprite;
                isTriangleUnavailable = true;
                t = 1.0f;
            }
        }
    }
}
