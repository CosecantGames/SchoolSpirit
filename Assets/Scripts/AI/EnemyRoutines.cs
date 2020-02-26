using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    //This holds all AIRoutines that enemies use
    public class EnemyRoutines : MonoBehaviour {
        Enemy me;

        public LookAround lookAround;
        public bool lookAroundRunning = false;
        public LookAt lookAt;
        public bool lookAtRunning = false;
        public LookAtPlayer lookAtPlayer;
        public bool lookAtPlayerRunning = false;

        private void Awake() {
            me = GetComponent<Enemy>();

            lookAround = me.gameObject.AddComponent<LookAround>();
            lookAt = me.gameObject.AddComponent<LookAt>();
            lookAtPlayer = me.gameObject.AddComponent<LookAtPlayer>();
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            lookAroundRunning = lookAround.isRunning;
            lookAtRunning = lookAt.isRunning;
            lookAtPlayerRunning = lookAtPlayer.isRunning;

        }

        public void KillAll(bool forceImmediate = true) {
            lookAround.Kill(forceImmediate);
            lookAt.Kill(forceImmediate);
            lookAtPlayer.Kill(forceImmediate);
        }
    }
}