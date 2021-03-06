﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlrCam : MonoBehaviour {
        public Camera plrCam;

        public bool lockCamera = true;

        public string mouseXInput = "Mouse X";
        public string mouseYInput = "Mouse Y";

        public float xSensitivity = 120f;
        public float ySensitivity = 120f;

        public Vector2 verticalMinMax;

        float vClamp = 0;

        //Vector3 camRotation = Vector3.zero;

        private void Awake() {
            if(lockCamera) {
                Cursor.lockState = CursorLockMode.Locked;
            }

            verticalMinMax = new Vector2(-85f, 85f);
        }

        // Start is called before the first frame update
        void Start() {
            plrCam = GetComponentInChildren<Camera>();
        }

        // Update is called once per frame
        void Update() {
            HandleMouseInput();
        }

        void HandleMouseInput() {
            float h = Input.GetAxis(mouseXInput) * (xSensitivity * Time.deltaTime);
            float v = Input.GetAxis(mouseYInput) * (ySensitivity * Time.deltaTime);
            Vector3 eulerRotation = Player.Cam.transform.eulerAngles;

            vClamp = Mathf.Clamp(vClamp + v, verticalMinMax.x, verticalMinMax.y);

            eulerRotation.x = -vClamp;
            Player.Cam.transform.eulerAngles = eulerRotation;
            transform.Rotate(Vector3.up * h);
        }
    }
}
