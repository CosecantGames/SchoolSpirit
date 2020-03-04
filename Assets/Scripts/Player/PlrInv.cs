using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlrInv : MonoBehaviour {
        //-1 = no item
        public int invIndex;
        public GameObject invObj;
        public Camera invCam;
        public List<GameObject> inv = new List<GameObject>();
        public float moveSpeed = 2f;
        public float invSpinSpeed = 1f;
        GameObject heldItem;

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
            if(Input.GetKeyDown(KeyCode.U)) {
                SetItemPositions();
            }

            float scrollInput = Input.GetAxisRaw("Scroll");

            if(scrollInput > 0) {
                if(invIndex < inv.Count - 1) {
                    invIndex++;
                }
            }

            if(scrollInput < 0) {
                if(invIndex >= 0) {
                    invIndex--;
                }
            }

            if(Input.GetKeyDown(KeyCode.X)) {
                Drop();
            }

            invCam.transform.localPosition = Vector3.MoveTowards(invCam.transform.localPosition, new Vector3(invIndex * 4, 0, -5f), moveSpeed);
        }

        public GameObject HeldItem {
            get => invIndex == -1 ? null : inv[invIndex];
        }

        public void Add(GameObject obj) {
            inv.Add(obj);
            obj.GetComponent<Rigidbody>().useGravity = false;
            obj.GetComponent<Rigidbody>().isKinematic = true;

            obj.transform.parent = invObj.transform;

            SetItemPositions();

            SpinObj spinObj = obj.AddComponent<SpinObj>();
            spinObj.speed = invSpinSpeed;
        }

        public void Hold() {

        }

        public void Drop() {
            if(invIndex >= 0) {
                GameObject obj = inv[invIndex];
                inv.RemoveAt(invIndex);
                SetItemPositions();

                obj.transform.parent = null;
                Destroy(obj.GetComponent<SpinObj>());
                obj.transform.position = Player.Cam.transform.position + (Player.Cam.transform.forward * 2);

                obj.GetComponent<Rigidbody>().useGravity = true;
                obj.GetComponent<Rigidbody>().isKinematic = false;

                if(invIndex > inv.Count - 1) {
                    invIndex--;
                }
            } else {
                Debug.Log("Not holding anything");
            }
        }

        void SetItemPositions() {
            for(int i = 0; i < inv.Count; i++) {
                inv[i].transform.localPosition = inv[i].GetComponent<PickupItem>().invPosition + new Vector3(4 * i, 0f, 0f);
                inv[i].transform.eulerAngles = inv[i].GetComponent<PickupItem>().invRotation;
            }
        }
    }
}