using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace TeamBronze.HexWars
{
    public class Store : MonoBehaviour
    {
        private PartAdder partAdder;
        private InputManager inputManager;
        private GameObject player;

        private bool isTriangleUnavailable;
        private float t = 1.0f;

        public Sprite triangleSprite;
        public Sprite noTriangleSprite;

        public GameObject store;

        public int storeMinimum = 3;

        void Start()
        {
            StartCoroutine(LateStart());
            store.SetActive(false);
        }
        
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
            if (player != null && (player.GetComponent<Player>().points >= storeMinimum && !store.GetActive()))
            {
                store.SetActive(true);

                if (!inputManager.IsActive()) 
                    return;

                Vector2 pos = -inputManager.lastMoveVector();
            }

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

        public void addHexagon()
        {

            if(partAdder.addRandomPart("Hexagon"))
            {
                player.GetComponent<Player>().points -= storeMinimum;

                store.SetActive(false);

                PhotonView destroyedView = PhotonView.Get(player);
                //destroyedView.RPC("updateServerScore", PhotonPlayer.Find(destroyedView.owner.ID));
            }

        }


        public void addTriangle()
        {

            if (partAdder.addRandomPart("Triangle"))
            {
                player.GetComponent<Player>().points -= storeMinimum;

                store.SetActive(false);

                PhotonView destroyedView = PhotonView.Get(player);
                //destroyedView.RPC("updateServerScore", PhotonPlayer.Find(destroyedView.owner.ID));
            }
            else
            {
                GameObject.FindGameObjectWithTag("TriangleStoreIcon").GetComponent<Image>().sprite = noTriangleSprite;
                isTriangleUnavailable = true;
                t = 1.0f;
            }
        }
    }
}
