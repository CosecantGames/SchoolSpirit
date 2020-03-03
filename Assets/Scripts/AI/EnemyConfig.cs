using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class EnemyConfig : MonoBehaviour {
        [Header("Chasing")]
        public float chaseVisRangeNear = 4f;
        public float chaseVisRangeMid = 15f;
        public float chaseVisRangeFar = 30f;
        public float chaseVisAngle = 50f;
        [Space(5)]
        public float chaseSpeed = 6f;
        public float chaseAngularSpeed = 450f;
        public float chaseAcceleration = 8f;
        public float chaseStoppingDistance = 0f;
        public bool chaseAutoBraking = false;
        public Config Chase = new Config();

        [Header("Searching")]
        public float searchVisRangeNear = 4f;
        public float searchVisRangeMid = 8f;
        public float searchVisRangeFar = 25f;
        public float searchVisAngle = 120f;
        [Space(5)]
        public float searchSpeed = 4f;
        public float searchAngularSpeed = 300f;
        public float searchAcceleration = 8f;
        public float searchStoppingDistance = 0f;
        public bool searchAutoBraking = true;
        public Config Search = new Config();

        [Header("Looking")]
        public float lookVisRangeNear = 5f;
        public float lookVisRangeMid = 15f;
        public float lookVisRangeFar = 30f;
        public float lookVisAngle = 150f;
        [Space(5)]
        public float lookSpeed = 0f;
        public float lookAngularSpeed = 450f;
        public float lookAcceleration = 8f;
        public float lookStoppingDistance = 0f;
        public bool lookAutoBraking = false;
        public Config Look = new Config();

        [Header("Patrolling")]
        public float patrolVisRangeNear = 3f;
        public float patrolVisRangeMid = 10f;
        public float patrolVisRangeFar = 20f;
        public float patrolVisAngle = 90f;
        [Space(5)]
        public float patrolSpeed = 4.5f;
        public float patrolAngularSpeed = 450f;
        public float patrolAcceleration = 8f;
        public float patrolStoppingDistance = 0f;
        public bool patrolAutoBraking = false;
        public Config Patrol = new Config();

        [Header("Awareness Speeds")]
        public float slowAware = 0.3f;
        public float medAware = 0.75f;
        public float fastAware = 1f;

        public Config[] config = new Config[4];

        public float[][] awarenessSpeeds = new float[4][];

        private void Awake() {
            Chase.visRangeNear = chaseVisRangeNear;
            Chase.visRangeMid = chaseVisRangeMid;
            Chase.visRangeFar = chaseVisRangeFar;
            Chase.visAngle = chaseVisAngle;
            Chase.speed = chaseSpeed;
            Chase.angularSpeed = chaseAngularSpeed;
            Chase.acceleration = chaseAcceleration;
            Chase.stoppingDistance = chaseStoppingDistance;
            Chase.autoBraking = chaseAutoBraking;

            Chase.awarenessSpeeds = new float[4][];
            Chase.awarenessSpeeds[0] = new float[4] { 0f, slowAware, medAware, Mathf.Infinity };
            Chase.awarenessSpeeds[1] = new float[4] { 0f, medAware, fastAware, Mathf.Infinity };
            Chase.awarenessSpeeds[2] = new float[4] { slowAware, medAware, fastAware, Mathf.Infinity };
            Chase.awarenessSpeeds[3] = new float[4] { fastAware, fastAware, Mathf.Infinity, Mathf.Infinity };
            config[0] = Chase;

            Search.visRangeNear = searchVisRangeNear;
            Search.visRangeMid = searchVisRangeMid;
            Search.visRangeFar = searchVisRangeFar;
            Search.visAngle = searchVisAngle;
            Search.speed = searchSpeed;
            Search.angularSpeed = searchAngularSpeed;
            Search.acceleration = searchAcceleration;
            Search.stoppingDistance = searchStoppingDistance;
            Search.autoBraking = searchAutoBraking;

            Search.awarenessSpeeds = new float[4][];
            Search.awarenessSpeeds[0] = new float[4] { 0f, slowAware, medAware, Mathf.Infinity };
            Search.awarenessSpeeds[1] = new float[4] { 0f, medAware, fastAware, Mathf.Infinity };
            Search.awarenessSpeeds[2] = new float[4] { slowAware, medAware, fastAware, Mathf.Infinity };
            Search.awarenessSpeeds[3] = new float[4] { fastAware, fastAware, Mathf.Infinity, Mathf.Infinity };
            config[1] = Search;

            Look.visRangeNear = lookVisRangeNear;
            Look.visRangeMid = lookVisRangeMid;
            Look.visRangeFar = lookVisRangeFar;
            Look.visAngle = lookVisAngle;
            Look.speed = lookSpeed;
            Look.angularSpeed = lookAngularSpeed;
            Look.acceleration = lookAcceleration;
            Look.stoppingDistance = lookStoppingDistance;
            Look.autoBraking = lookAutoBraking;

            Look.awarenessSpeeds = new float[4][];
            Look.awarenessSpeeds[0] = new float[4] { medAware, fastAware, Mathf.Infinity, Mathf.Infinity };
            Look.awarenessSpeeds[1] = new float[4] { fastAware, fastAware, Mathf.Infinity, Mathf.Infinity };
            Look.awarenessSpeeds[2] = new float[4] { fastAware, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
            Look.awarenessSpeeds[3] = new float[4] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };
            config[2] = Look;

            Patrol.visRangeNear = patrolVisRangeNear;
            Patrol.visRangeMid = patrolVisRangeMid;
            Patrol.visRangeFar = patrolVisRangeFar;
            Patrol.visAngle = patrolVisAngle;
            Patrol.speed = patrolSpeed;
            Patrol.angularSpeed = patrolAngularSpeed;
            Patrol.acceleration = patrolAcceleration;
            Patrol.stoppingDistance = patrolStoppingDistance;
            Patrol.autoBraking = patrolAutoBraking;

            Patrol.awarenessSpeeds = new float[4][];
            Patrol.awarenessSpeeds[0] = new float[4] { 0f, slowAware, medAware, Mathf.Infinity };
            Patrol.awarenessSpeeds[1] = new float[4] { 0f, medAware, fastAware, Mathf.Infinity };
            Patrol.awarenessSpeeds[2] = new float[4] { slowAware, medAware, fastAware, Mathf.Infinity };
            Patrol.awarenessSpeeds[3] = new float[4] { fastAware, fastAware, Mathf.Infinity, Mathf.Infinity };
            config[3] = Patrol;
        }
    }

    public class Config {
        public float visRangeNear;
        public float visRangeMid;
        public float visRangeFar;
        public float visAngle;
        public float speed;
        public float angularSpeed;
        public float acceleration;
        public float stoppingDistance;
        public bool autoBraking;
        public float[][] awarenessSpeeds;
    }
}