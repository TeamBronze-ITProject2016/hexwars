using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace TeamBronze.HexWars
{
    public class Store : MonoBehaviour
    {
        private PartAdder partAdder;
        private InputManager inputManager;
        private GameObject player;

        public GameObject hexagonStore;
        public GameObject triangleStore;

        public GameObject store;

        public int storeMinimum = 3;

        void Start()
        {
            StartCoroutine(LateStart());
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
                store.SetActive(true);
        }

        public void addHexagon()
        {

            partAdder.addRandomPart("Hexagon");

            player.GetComponent<Player>().points -= storeMinimum;

            store.SetActive(false);

            PhotonView destroyedView = PhotonView.Get(player);
            destroyedView.RPC("updateServerScore", PhotonPlayer.Find(destroyedView.owner.ID));
        }


        public void addTriangle()
        {

            partAdder.addRandomPart("Triangle");

            player.GetComponent<Player>().points -= storeMinimum;

            store.SetActive(false);

            PhotonView destroyedView = PhotonView.Get(player);
            destroyedView.RPC("updateServerScore", PhotonPlayer.Find(destroyedView.owner.ID));
        }
    }
}
