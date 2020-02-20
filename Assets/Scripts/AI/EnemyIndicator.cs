using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    //This script deals with changing the enemy's color based on its state
    public class EnemyIndicator : MonoBehaviour {
        Enemy me;
        public MeshRenderer mesh;

        private void Awake() {
            mesh = GetComponent<MeshRenderer>();
        }

        // Start is called before the first frame update
        void Start() {
            me = GetComponent<Enemy>();
        }

        // Update is called once per frame
        void Update() {
            switch(me.state) {
                case EnemyStates.Chasing:
                    mesh.material.color = Color.red;
                    break;
                case EnemyStates.Searching:
                    mesh.material.color = Color.yellow;
                    break;
                case EnemyStates.Looking:
                    mesh.material.color = Color.magenta;
                    break;
                case EnemyStates.Patrolling:
                    mesh.material.color = Color.green;
                    break;
                default:
                    mesh.material.color = Color.white;
                    break;
            }
        }
    }
}
