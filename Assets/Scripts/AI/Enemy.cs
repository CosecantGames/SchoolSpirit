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
        [Header("Enemy Components")]
        public EnemyMove Move;
        public EnemyLook Look;
        public EnemyState State;
        public EnemyRoutines Routines;
        public EnemyConfig Configs;

        [Space(6)]
        public NavMeshAgent agent;
        public Config config;

        [Space(6)]
        public EnemyStates state;
        public EnemyStates lastState;   //State during previous frame
        public EnemyStates nextState = EnemyStates.Patrolling;   //State to go to after timing out idle

        [Space(6)]
        public bool ignorePlayer = false;
        public bool hasLineOfSight = false;
        public bool seesPlayer = false;

        [Header("Alert Speed Stuff")]
        public int playerProximity;
        public float awareness;

        public float searchTolerance = 5f;   //When awareness > this value, enemy starts searching
        public float alertTolerance = 8f;   //When awareness > this value, enemy starts chasing

        [Range(0f, 1f)]
        public float awarenessRiseScalar = 1f;  //How quickly awareness increases
        [Range(0f, 1f)]
        public float awarenessFallScalar = 1f;  //How quickly awareness decreases

        private void Awake() {
            Routines = GetComponent<EnemyRoutines>();
            Move = GetComponent<EnemyMove>();
            Look = GetComponent<EnemyLook>();
            State = GetComponent<EnemyState>();
            Configs = GetComponent<EnemyConfig>();

            agent = GetComponent<NavMeshAgent>();

            state = nextState;
            lastState = nextState;
        }

        public float stateTimer;
        public bool stateSwapped = false;

        private void FixedUpdate() {
            //if(seesPlayer) {
            //    awareness += Player.Player.visibility * Player.Player.visibility * awarenessRiseScalar *
            //        (1 - Global.Map(transform.position.flatDistTo(Global.Plr.transform.position), 0f, Look.visRange, 0f, 1f));
            //} else {
            //    awareness *= awarenessFallScalar;
            //    if(awareness < 0.01) {
            //        awareness = 0f;
            //    }
            //}

            //awareness = Mathf.Clamp(awareness, 0f, 100f);
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetButtonDown("BigRedButton")) {
                Debug.Log("PUSHED THE BIG RED BUTTON!!!");
                Routines.KillAll(true);
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

            Routines.KillAll(true);

            switch(state) {
                case EnemyStates.Chasing:
                    SwapAgent(EnemyAgents.chaseAgent);
                    config = Configs.Chase;

                    Routines.lookAtPlayer.Run(45f);

                    break;
                case EnemyStates.Searching:
                    SwapAgent(EnemyAgents.searchAgent);
                    config = Configs.Search;

                    break;
                case EnemyStates.Looking:
                    SwapAgent(EnemyAgents.searchAgent);
                    config = Configs.Look;

                    Routines.lookAround.Run();

                    break;
                case EnemyStates.Patrolling:
                    SwapAgent(EnemyAgents.patrolAgent);
                    config = Configs.Patrol;

                    break;
                case EnemyStates.Idle:
                    SwapAgent(EnemyAgents.idleAgent);
                    config = Configs.Search;

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
                    Routines.lookAround.Kill(true);
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
