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
        GameObject activeItem;

        public LayerMask mask;

        private void Awake() {
            player = GetComponent<Player>();
            activeItem = gameObject;
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetKeyDown(KeyCode.L) && Physics.Raycast(Player.Cam.transform.position, Player.Cam.transform.forward, out RaycastHit hitLM, useRange, mask)){
                Debug.Log(hitLM.transform.name);
                Vector2 lmCoords = hitLM.lightmapCoord;
                Renderer hitRenderer = hitLM.transform.GetComponent<Renderer>();
                Debug.Log(LightmapSettings.lightmaps[hitRenderer.lightmapIndex].shadowMask.GetPixel((int)lmCoords.x, (int) lmCoords.y));
            }

            usePressed = Input.GetButtonDown(useInput);

            //This detects things on layer 9. Change the bitmask (1 << 9) to change the layer it's active for.
            if(Physics.Raycast(Player.Cam.transform.position, Player.Cam.transform.forward, out RaycastHit hit, useRange, 1 << 9)) {
                activeItem = hit.transform.gameObject;
                foreach(Material mat in activeItem.GetComponent<MeshRenderer>().materials) {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", new Color(0.1f, 0.1f, 0.1f));
                }

                if(usePressed) {
                    try {
                        hit.transform.gameObject.SendMessage("Use");
                    } catch {
                        throw new Exception("Usable object does not have Use() method. Attach a script with one.");
                    }
                }
            } else {
                foreach(Material mat in activeItem.GetComponent<MeshRenderer>().materials) {
                    mat.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}
