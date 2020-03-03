using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlrInv : MonoBehaviour {
        public int invIndex;
        public GameObject invObj;
        public Camera invCam;
        public List<GameObject> inv = new List<GameObject>();
        public float moveSpeed = 2f;
        public GameObject heldItem;

        // Start is called before the first frame update
        void Start() {
            invObj = GameObject.Find("Inv");
            invCam = invObj.GetComponentInChildren<Camera>();

            Transform[] invObjs = invObj.GetComponentsInChildren<Transform>();
            foreach(Transform obj in invObjs) {
                if(obj.gameObject.layer == 14) {
                    inv.Add(obj.gameObject);
                }
            }
        }

        // Update is called once per frame
        void Update() {
            float scrollInput = Input.GetAxisRaw("Scroll");

            if(scrollInput > 0) {
                if(invIndex < inv.Count) {
                    invIndex++;
                    heldItem = inv[invIndex - 1];
                }
            }

            if(scrollInput < 0) {
                if(invIndex > 0) {
                    invIndex--;
                    if(invIndex > 0) {
                        heldItem = inv[invIndex - 1];
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.X)) {
                Drop();
            }

            invCam.transform.localPosition = Vector3.MoveTowards(invCam.transform.localPosition, new Vector3(invIndex * 4, 0, -5f), moveSpeed);
        }

        public void Add(GameObject obj) {
            inv.Add(obj);
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<Rigidbody>().isKinematic = true;

            obj.transform.parent = invObj.transform;

            obj.transform.position = Vector3.zero;
            obj.transform.localPosition = new Vector3(inv.Count * 4, 0f, 0f);
            obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            obj.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            obj.AddComponent<SpinObj>();
        }

        public void Hold() {

        }

        public void Drop() {
            if(invIndex != 0) {
                GameObject obj = inv[invIndex - 1];
                obj.transform.parent = null;
                Destroy(obj.GetComponent<SpinObj>());
                obj.transform.position = Player.Cam.transform.position + (Player.Cam.transform.forward * 2);

                obj.GetComponent<Rigidbody>().useGravity = true;
                obj.GetComponent<Rigidbody>().isKinematic = false;

                inv.RemoveAt(invIndex - 1);

                invIndex--;
            } else {
                Debug.Log("Not holding anything");
            }
        }
    }
}