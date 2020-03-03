using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    //This script deals with whether the enemy has the player in its line of sight
    public class EnemyLook : MonoBehaviour {
        Enemy me;

        public bool showVis = false;
        public bool logAngle = false;

        public Vector3 vectorToPlr = Vector3.zero;

        public float angleToPlr;
        public int playerProximity = 0;

        private void Awake() {
            me = GetComponent<Enemy>();
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            //Gets direction vector from enemy to player
            vectorToPlr = Global.Plr.transform.position - transform.position;
            angleToPlr = Vector3.Angle(transform.forward, vectorToPlr);
            
            LookForPlayer();
        }

        public void LookForPlayer() {
            bool inVisCone = angleToPlr <= me.config.visAngle / 2;
            me.hasLineOfSight = me.sees(Global.Plr) && inVisCone;

            Physics.Raycast(transform.position, Global.Plr.transform.position - transform.position, out RaycastHit hit);

            if(hit.distance < me.config.visRangeNear) {
                playerProximity = 3;
            } else if(hit.distance < me.config.visRangeMid) {
                playerProximity = 2;
            } else if(hit.distance < me.config.visRangeFar) {
                playerProximity = 1;
            } else {
                playerProximity = 0;
            }

            me.playerProximity = playerProximity;
        }

        private void OnDrawGizmosSelected() {
            if(showVis) {
                vectorToPlr = Global.Plr.transform.position - transform.position;

                Vector3 fwdBound = transform.forward * me.config.visRangeFar;
                Vector3 leftBound = Quaternion.Euler(0f, me.config.visAngle / -2, 0f) * fwdBound;
                Vector3 rightBound = Quaternion.Euler(0f, me.config.visAngle / 2, 0f) * fwdBound;

                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + fwdBound);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + leftBound);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + rightBound);

                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, Global.Plr.transform.position);
            }
        }

        private void OnValidate() {
            if(logAngle) {
                Debug.Log(Vector3.Angle(transform.forward, vectorToPlr));
            }

            logAngle = false;
        }
    }
}