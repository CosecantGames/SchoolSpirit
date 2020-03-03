using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class EnemyMove : MonoBehaviour {
        public Enemy me;

        public Transform target;

        private void Awake() {
            me = GetComponent<Enemy>();
            target = transform;
        }

        public void Chase() {
            if(me.stateSwapped) {
                me.stateSwapped = false;

                me.chaseTimer = 0f;
                //me.Routines.lookAtPlayer.Run(30f);
            }

            target = Global.Plr.transform;
            me.agent.destination = target.position;
            me.chaseTimer += Time.deltaTime;

            if(!me.hasLineOfSight) {
                me.Routines.lookAtPlayer.Kill();
                me.SwapState(EnemyStates.Searching, me.chaseTimer * 2);
            }
        }

        [Header("Searching Stuff")]
        public float searchDist;
        public Vector3 lastPlayerPos;
        public void Search() {
            if(me.stateSwapped) {
                me.stateSwapped = false;

                List<Node> searchRoute;
                lastPlayerPos = Global.Plr.transform.position;
                me.agent.destination = lastPlayerPos;
            }

            searchDist = transform.position.flatDistTo(lastPlayerPos);
            Debug.DrawLine(transform.position, lastPlayerPos);

            if(transform.position.flatDistTo(lastPlayerPos) <= 0.5) {
                me.SwapState(EnemyStates.Looking, 4f);
            }
        }

        [Header("Patrol Settings")]
        public List<Node> patrolRoute;
        List<Node> defaultPatrolRoute;
        public RouteType routeType = RouteType.Loop;
        public Node targetNode;
        public int routeIndex = 0;

        public float remainingDist = 0f;

        public void Patrol() {
            if(me.stateSwapped) {
                me.stateSwapped = false;
            }

            if(patrolRoute.Count < 1) {
                return;
            }

            remainingDist = transform.position.flatDistTo(target.position);

            if(remainingDist <= 1) {
                routeIndex = routeIndex < patrolRoute.Count - 1 ? routeIndex + 1 : 0;
                targetNode = patrolRoute[routeIndex];
            }

            try {
                target = targetNode.transform;
            } catch {
                throw new System.Exception("Could not patrol; nodes in route do not exist");
            }

            me.agent.destination = target.position;
        }
    }

}