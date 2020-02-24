using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    //This holds all AIRoutines that enemies use
    public class EnemyRoutines : MonoBehaviour {
        Enemy me;

        public LookAround lookAround;
        public LookAt lookAt;
        public LookAtPlayer lookAtPlayer;

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

        }

        public void KillAll(bool forceImmediate = false) {
            lookAround.Kill(forceImmediate);
            lookAt.Kill(forceImmediate);
            lookAtPlayer.Kill(forceImmediate);
        }
    }
}