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

        public bool isStoreActive = true;

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
            if (isStoreActive) storeActive();
        }

        void storeActive()
        {
            // If the store is active, freeze the player until a part has been attached
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.Euler(Vector3.zero);

            // If user clicks on a part of the screen, attach a hexagon to the closest part of that player
            if (!inputManager.IsActive()) return;

            Vector2 pos = -inputManager.lastMoveVector();

            partAdder.addPart(pos, "Hexagon");

            isStoreActive = false;
        }
    }
}
