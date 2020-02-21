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
        public EnemyStates nextState = EnemyStates.Patrolling;   //State to go to after timing out idle

        public bool seesPlayer = false;

        public Transform target;

        private void Awake() {
            //Move = GetComponent<EnemyMove>();
            Look = GetComponent<EnemyLook>();
            //Patrol = GetComponent<EnemyPatrol>();
            //Chase = GetComponent<EnemyChase>();

            agent = GetComponent<NavMeshAgent>();

            state = nextState;
            lastState = nextState;
        }

        public KeyCode startLook = KeyCode.O;
        public KeyCode startLookPlr = KeyCode.P;

        [Header("AIRoutines")]
        public AIRoutine[] routineQueue;
        public LookAt lookAt;
        public LookAtPlayer lookAtPlayer;
        public LookAround lookAround;

        public float stateTimer;
        bool stateSwapped = false;

        // Start is called before the first frame update
        void Start() {
            lookAt = gameObject.AddComponent<LookAt>();
            lookAtPlayer = gameObject.AddComponent<LookAtPlayer>();
            lookAround = gameObject.AddComponent<LookAround>();
            target = transform;
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetButtonDown("BigRedButton")) {
                Debug.Log("PUSHED THE BIG RED BUTTON!!!");
                lookAt.Kill();
                lookAtPlayer.Kill();
                lookAround.Kill();
            }

            if(Input.GetKeyDown(startLook)) {
                state = EnemyStates.Looking;
                stateTimer = 6f;
                lookAround.Run(6f, 130f, 6f);
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
                    Search();
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

            stateTimer -= Time.deltaTime;

            if(stateTimer <= 0) {
                Timeout();
            }

            lastState = state;
        }

        void HandleState() {
            if(seesPlayer && state != EnemyStates.Chasing) {
                SwapState(EnemyStates.Chasing);
            }
        }

        public void SwapAgent(NavMeshAgent newAgent) {
            Debug.Log("Swapping Agent to " + newAgent.name);

            agent.speed = newAgent.speed;
            agent.angularSpeed = newAgent.angularSpeed;
            agent.acceleration = newAgent.acceleration;
            agent.stoppingDistance = newAgent.stoppingDistance;
            agent.autoBraking = newAgent.autoBraking;
        }

        float chaseTimer = 0f;
        public void Chase() {
            if(stateSwapped) {
                stateSwapped = false;

                chaseTimer = 0f;
                lookAtPlayer.Run(30f);
            }

            target = Global.Plr.transform;
            agent.destination = target.position;
            chaseTimer += Time.deltaTime;

            if(!seesPlayer) {
                lookAtPlayer.Kill();
                SwapState(EnemyStates.Searching, 10f);
            }
        }

        [Header("Searching Stuff")]

        public float searchDist;
        public Vector3 lastPlayerPos;
        public void Search() {
            if(stateSwapped) {
                stateSwapped = false;

                List<Node> searchRoute;
                Debug.Log(Global.Plr.transform.position);
                lastPlayerPos = Global.Plr.transform.position;
                agent.destination = lastPlayerPos;
            }
            
            searchDist = transform.position.flatDistTo(lastPlayerPos);
            Debug.DrawLine(transform.position, lastPlayerPos);

            if(transform.position.flatDistTo(lastPlayerPos) <= 0.5) {
                SwapState(EnemyStates.Looking, 4f);
            }
        }

        [Header("Patrol Settings")]

        public List<Node> patrolRoute;
        public RouteType routeType = RouteType.Loop;
        public Node targetNode;
        public int routeIndex = 0;

        public float remainingDist = 0f;

        public void Patrol() {
            if(stateSwapped) {
                stateSwapped = false;
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

        //Enemy State management
        public void SwapState(EnemyStates newState = EnemyStates.Patrolling, float time = 5f) {
            state = newState;
            stateSwapped = true;

            switch(state) {
                case EnemyStates.Chasing:
                    lookAround.Kill();

                    lookAtPlayer.Run(45f);
                    SwapAgent(EnemyAgents.chaseAgent);
                    Look.visAngle = 30f;
                    Look.visRange = 30f;
                    break;
                case EnemyStates.Searching:
                    SwapAgent(EnemyAgents.searchAgent);
                    Look.visAngle = 75f;
                    Look.visRange = 30f;
                    break;
                case EnemyStates.Looking:
                    SwapAgent(EnemyAgents.searchAgent);
                    lookAround.Run();
                    Look.visAngle = 105f;
                    Look.visRange = 30f;
                    break;
                case EnemyStates.Patrolling:
                    SwapAgent(EnemyAgents.patrolAgent);
                    Look.visAngle = 40f;
                    Look.visRange = 30f;
                    break;
                case EnemyStates.Idle:
                    SwapAgent(EnemyAgents.idleAgent);
                    Look.visAngle = 75f;
                    Look.visRange = 16f;
                    break;
                default:
                    break;
            }

            stateTimer = time;
        }

        public void Timeout() {
            switch(state) {
                case EnemyStates.Chasing:
                    break;
                case EnemyStates.Searching:
                    SwapState(EnemyStates.Looking);
                    break;
                case EnemyStates.Looking:
                    lookAround.Kill();
                    SwapState(EnemyStates.Idle, 1.5f);
                    nextState = EnemyStates.Patrolling;
                    break;
                case EnemyStates.Patrolling:
                    SwapState(EnemyStates.Patrolling);
                    break;
                case EnemyStates.Idle:
                    state = nextState;
                    break;
                default:
                    break;
            }
        }
    }
}
