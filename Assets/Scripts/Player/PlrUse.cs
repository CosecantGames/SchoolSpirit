using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlrUse : MonoBehaviour {
        public Player player;

        [Range(0f, 10f)]
        public float useRange = 6f;
        public string useInput = "Use";
        bool usePressed;

        private void Awake() {
            player = GetComponent<Player>();
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            usePressed = Input.GetButtonDown(useInput);

            if(usePressed) {
                //This detects things on layer 9. Change the bitmask (1 << 9) to change the layer it's active for.
                if(Physics.Raycast(Player.Cam.transform.position, Player.Cam.transform.forward, out RaycastHit hit, useRange, 1 << 9)) {
                    try {
                        hit.transform.gameObject.SendMessage("Use", gameObject);
                    } catch {
                        throw new Exception("Usable object does not have Use() method. Attach a script with one.");
                    }
                }
            }
        }
    }
}
