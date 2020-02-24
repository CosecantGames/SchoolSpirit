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

        public float visibility = 1f;

        public static PlayerStates state;
        
        // Start is called before the first frame update
        void Awake() {
            Cam = GetComponentInChildren<Camera>();
            Move = GetComponent<PlrMove>();
            Use = GetComponent<PlrUse>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}