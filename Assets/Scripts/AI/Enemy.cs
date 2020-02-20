using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    public enum EnemyStates {
        Chasing,    //Chasing = Directly following the player
        Searching,  //Searching = Walking around where the player was last seen to find them
        Looking,    //Looking = Rotating on the spot to find the player
        Patrolling, //Patrolling = Moving along a patrol route
        Idle
    }

    public enum RouteType {
        Loop,
        Line
    }

    public class Enemy : MonoBehaviour {
        //public EnemyMove Move;
        public EnemyLook Look;
        //public EnemyPatrol Patrol;
        //public EnemyChase Chase;

        public NavMeshAgent agent;

        public EnemyStates state;
        public EnemyStates lastState;   //State during previous frame

        public bool seesPlayer = false;

        public Transform target;

        private void Awake() {
            //Move = GetComponent<EnemyMove>();
            Look = GetComponent<EnemyLook>();
            //Patrol = GetComponent<EnemyPatrol>();
            //Chase = GetComponent<EnemyChase>();

            agent = GetComponent<NavMeshAgent>();

            state = EnemyStates.Idle;
            lastState = EnemyStates.Idle;
        }

        public KeyCode startLook = KeyCode.O;
        public KeyCode startLookPlr = KeyCode.P;

        [Header("AIRoutines")]
        public AIRoutine[] routineQueue;
        public LookAt lookAt;
        public LookAtPlayer lookAtPlayer;

        // Start is called before the first frame update
        void Start() {
            lookAt = gameObject.AddComponent<LookAt>();
            lookAtPlayer = gameObject.AddComponent<LookAtPlayer>();
            target = transform;
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetButtonDown("BigRedButton")) {
                Debug.Log("PUSHED THE BIG RED BUTTON!!!");
                StopAllCoroutines();
            }

            if(Input.GetKeyDown(startLook)) {
                lookAt.Run(Global.Plr.transform.position);
            }

            if(Input.GetKeyDown(startLookPlr)) {
                lookAtPlayer.Run();
            }
            
            HandleState();

            switch(state) {
                case EnemyStates.Chasing:
                    Chase();
                    break;
                case EnemyStates.Searching:
                    break;
                case EnemyStates.Looking:
                    break;
                case EnemyStates.Patrolling:
                    Patrol();
                    break;
                case EnemyStates.Idle:
                    break;
                default:
                    break;
            }

            lastState = state;
        }

        void HandleState() {
            //if(Mathf.Sign(Look.distToPlr) == -1) {
            //    state = EnemyStates.Idle;
            //} else if(Look.distToPlr <= Look.visRange / 2) {
            //    state = EnemyStates.Chasing;
            //} else if(Look.distToPlr <= Look.visRange) {
            //    state = EnemyStates.Searching;
            //} else {
            //    state = EnemyStates.Patrolling;
            //}

            if(seesPlayer) {
                state = EnemyStates.Chasing;
                Look.visAngle = 75f;
                Look.visRange = 30f;
            } else {
                state = EnemyStates.Patrolling;
                Look.visAngle = 30f;
                Look.visRange = 16f;
            }
        }

        public void SwapAgent(NavMeshAgent newAgent) {
            //System.Type type = agent.GetType();
            //System.Reflection.FieldInfo[] fields = type.GetFields();

            //foreach(System.Reflection.FieldInfo field in fields) {
            //    field.SetValue(agent, field.GetValue(newAgent));
            //}

            Debug.Log("Swapping Agent to " + newAgent.name);

            agent.speed = newAgent.speed;
            agent.angularSpeed = newAgent.angularSpeed;
            agent.acceleration = newAgent.acceleration;
            agent.stoppingDistance = newAgent.stoppingDistance;
            agent.autoBraking = newAgent.autoBraking;
        }

        public void Chase() {
            if(state != lastState) {
                SwapAgent(EnemyAgents.chaseAgent);
                lookAtPlayer.Run(24f);
            }

            target = Global.Plr.transform;

            agent.destination = target.position;
        }

        [Header("Patrol Settings")]

        public List<Node> patrolRoute;
        public RouteType routeType = RouteType.Loop;
        public Node targetNode;
        public int routeIndex = 0;

        public float remainingDist = 0f;

        public void Patrol() {
            if(state != lastState) {
                SwapAgent(EnemyAgents.patrolAgent);
            }

            if(patrolRoute.Count < 1) {
                return;
            }

            remainingDist = transform.position.flatDistTo(target.position);

            if(remainingDist <= 1) {
                routeIndex = routeIndex < patrolRoute.Count - 1 ? routeIndex + 1 : 0;
                targetNode = patrolRoute[routeIndex];
            }

            target = targetNode.transform;

            agent.destination = target.position;
        }
    }
}
