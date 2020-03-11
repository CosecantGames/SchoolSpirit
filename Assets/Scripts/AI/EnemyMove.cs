using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    public class EnemyMove : MonoBehaviour {
        public Enemy me;

        public Vector3 target;

        public NavMeshPathStatus status;

        private void Awake() {
            me = GetComponent<Enemy>();
            target = transform.position;
        }

        public void Chase() {
            if(me.stateSwapped) {
                me.stateSwapped = false;

                me.chaseTimer = 0f;
                //me.Routines.lookAtPlayer.Run(30f);
            }

            target = Global.Plr.transform.position;
            me.agent.destination = target;
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

        float pathTimeout = 3f;

        public void Patrol() {
            if(me.stateSwapped) {
                me.stateSwapped = false;
            }

            if(patrolRoute.Count < 1) {
                return;
            }

            pathTimeout -= Time.deltaTime;

            remainingDist = transform.position.flatDistTo(target);

            if(remainingDist <= 1 || pathTimeout <= 0) {
                pathTimeout = 200f;

                if(me.agent.path.status == NavMeshPathStatus.PathPartial) {
                    Debug.DrawLine(transform.position, targetNode.transform.position, Color.red, 1f);
                    me.agent.SamplePathPosition(1 << 4, 2f, out NavMeshHit nmh);
                    Debug.Log(nmh.mask);
                    if(Physics.Linecast(transform.position, targetNode.transform.position, out RaycastHit hit, 1 << 9)) {
                        if(hit.transform.CompareTag("Door") && hit.distance <= 4) {
                            OpenDoor door = hit.transform.GetComponent<OpenDoor>();
                            if(door.IsLocked == false) {
                                door.isOpen = true;
                                door.autoClose = true;
                                door.closeTimer = 1.5f;
                                routeIndex--;
                            }
                        }
                    }
                }

                routeIndex = routeIndex < patrolRoute.Count - 1 ? routeIndex + 1 : 0;
                targetNode = patrolRoute[routeIndex];

                if(targetNode == null) {
                    throw new System.Exception("Could not patrol; nodes in route do not exist");
                }

                NavMeshPath path = new NavMeshPath();
                me.agent.CalculatePath(targetNode.transform.position, path);

                switch(path.status) {
                    case NavMeshPathStatus.PathComplete:
                        target = targetNode.transform.position;
                        break;
                    case NavMeshPathStatus.PathPartial:
                        Debug.Log(name + " only found partial route to " + targetNode.name);
                        target = path.corners[path.corners.Length - 1];
                        break;
                    case NavMeshPathStatus.PathInvalid:
                        Debug.Log(name + " has no route to " + targetNode.name);
                        target = transform.position;
                        break;
                    default:
                        break;
                }

                me.agent.SetPath(path);
            }

            status = me.agent.path.status;
        }
    }

}