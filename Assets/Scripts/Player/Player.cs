using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public enum PlayerStates {
        Idle,
        Walking,
        Running,
        Jumping,
        Aerial,
        Crouching
    }

    public class Player : MonoBehaviour {
        public static PlrMove Move;
        public static Camera Cam;
        public static PlrUse Use;
        public static PlrInv Inv;

        public static SphereCollider Head;

        public static int lightLevel = 0;

        public static PlayerStates state;
        
        // Start is called before the first frame update
        void Awake() {
            Cam = GetComponentInChildren<Camera>();
            Move = GetComponent<PlrMove>();
            Use = GetComponent<PlrUse>();
            Inv = GetComponent<PlrInv>();

            Head = GetComponentInChildren<SphereCollider>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}