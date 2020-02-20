using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class EnemyState : MonoBehaviour {
        Enemy me;

        private void Awake() {
            me = GetComponent<Enemy>();
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void Swap() {
            switch(me.state) {
                case EnemyStates.Patrolling:
                    break;
                case EnemyStates.Chasing:
                    break;
                case EnemyStates.Searching:
                    break;
                case EnemyStates.Idle:
                    break;
                default:
                    break;
            }
        }

        public void Timeout() {
            switch(me.state) {
                case EnemyStates.Chasing:
                    break;
                case EnemyStates.Searching:
                    me.state = EnemyStates.Looking;
                    break;
                case EnemyStates.Looking:
                    me.state = EnemyStates.Patrolling;
                    break;
                case EnemyStates.Patrolling:
                    break;
                case EnemyStates.Idle:
                    break;
                default:
                    break;
            }
        }
    }
}
