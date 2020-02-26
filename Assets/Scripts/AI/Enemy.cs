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
        public EnemyMove Move;
        public EnemyLook Look;
        public EnemyState State;

        public NavMeshAgent agent;

        public EnemyStates state;
        public EnemyStates lastState;   //State during previous frame
        public EnemyStates nextState = EnemyStates.Patrolling;   //State to go to after timing out idle

        public bool ignorePlayer = false;
        public bool seesPlayer = false;

        [Header("Alert Speed Stuff")]
        public float awareness;

        public float searchTolerance = 5f;   //When awareness > this value, enemy starts searching
        public float alertTolerance = 8f;   //When awareness > this value, enemy starts chasing

        [Range(0f, 1f)]
        public float awarenessRiseScalar = 1f;  //How quickly awareness increases
        [Range(0f, 1f)]
        public float awarenessFallScalar = 1f;  //How quickly awareness decreases

        private void Awake() {
            routines = GetComponent<EnemyRoutines>();
            Move = GetComponent<EnemyMove>();
            Look = GetComponent<EnemyLook>();
            State = GetComponent<EnemyState>();

            agent = GetComponent<NavMeshAgent>();

            state = nextState;
            lastState = nextState;
        }

        public KeyCode startLook = KeyCode.O;
        public KeyCode startLookPlr = KeyCode.P;

        [Header("AIRoutines")]
        public AIRoutine[] routineQueue;
        public EnemyRoutines routines;

        public float stateTimer;
        public bool stateSwapped = false;

        public float alertTimer = 0f;

        private void FixedUpdate() {
            if(seesPlayer) {
                awareness += Player.Player.visibility * Player.Player.visibility * awarenessRiseScalar *
                    (1 - Global.Map(transform.position.flatDistTo(Global.Plr.transform.position), 0f, Look.visRange, 0f, 1f));
            } else {
                awareness *= awarenessFallScalar;
                if(awareness < 0.01) {
                    awareness = 0f;
                }
            }

            awareness = Mathf.Clamp(awareness, 0f, 100f);

        }

        // Update is called once per frame
        void Update() {
            if(Input.GetButtonDown("BigRedButton")) {
                Debug.Log("PUSHED THE BIG RED BUTTON!!!");
                routines.KillAll(true);
            }

            if(Input.GetKeyDown(startLook)) {
                state = EnemyStates.Looking;
                stateTimer = 6f;
                routines.lookAround.Run(6f, 130f, 6f);
            }

            if(Input.GetKeyDown(startLookPlr)) {
                routines.lookAtPlayer.Run();
            }
            
            HandleState();

            switch(state) {
                case EnemyStates.Chasing:
                    Move.Chase();
                    break;
                case EnemyStates.Searching:
                    Move.Search();
                    break;
                case EnemyStates.Looking:
                    break;
                case EnemyStates.Patrolling:
                    Move.Patrol();
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
            if(ignorePlayer) {
                return;
            }

            if(awareness > alertTolerance) {
                if(state != EnemyStates.Chasing) {
                    SwapState(EnemyStates.Chasing);
                }
            } else if(awareness > searchTolerance) {
                if(state != EnemyStates.Searching) {
                    SwapState(EnemyStates.Searching);
                }
            }
        }

        public void SwapAgent(NavMeshAgent newAgent) {
            agent.speed = newAgent.speed;
            agent.angularSpeed = newAgent.angularSpeed;
            agent.acceleration = newAgent.acceleration;
            agent.stoppingDistance = newAgent.stoppingDistance;
            agent.autoBraking = newAgent.autoBraking;
        }

        public float chaseTimer = 0f;
        
        //Enemy State management
        public void SwapState(EnemyStates newState = EnemyStates.Patrolling, float time = 5f) {
            state = newState;
            stateSwapped = true;

            routines.KillAll(true);

            switch(state) {
                case EnemyStates.Chasing:
                    routines.lookAround.Kill(true);

                    routines.lookAtPlayer.Run(45f);
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
                    routines.lookAround.Run();
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
                    routines.lookAround.Kill(true);
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
