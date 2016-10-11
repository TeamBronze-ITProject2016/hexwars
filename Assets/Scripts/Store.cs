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
            yield return new WaitForSeconds(1);
            inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
            partAdder = GameObject.FindGameObjectWithTag("PartAdder").GetComponent<PartAdder>();
            player = GameObject.FindGameObjectWithTag("LocalPlayer");
        }

        // Update is called once per frame
        void Update()
        {
            if (player.GetComponent<Player>().points >= storeMinimum && !store.GetActive())
                store.SetActive(true);
        }

        public void addHexagon()
        {
            // freeze the player until a part has been attached
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);

            partAdder.addRandomPart("Hexagon");

            player.GetComponent<Player>().points -= storeMinimum;

            store.SetActive(false);
        }


        public void addTriangle()
        {
            // freeze the player until a part has been attached
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);

            partAdder.addRandomPart("Triangle");

            player.GetComponent<Player>().points -= storeMinimum;

            store.SetActive(false);
        }
    }
}
