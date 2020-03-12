using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player {
    public class PlrVisibility : MonoBehaviour {
        public GameObject target;
        public List<GameObject> hitList;

        public Player player;
        public Enemy.Enemy enemy;
        public Enemy.Enemy enemyB;
        public TextMeshProUGUI visMeter;

        private void Awake() {
            player = GetComponent<Player>();
            //enemy = GameObject.Find("Enemy").GetComponent<Enemy.Enemy>();
            //enemyB = GameObject.Find("Enemy (1)").GetComponent<Enemy.Enemy>();
        }

        // Start is called before the first frame update
        void Start() {
            visMeter = GameObject.Find("VisMeter").GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update() {
            //FindLights();
            CalcLights();
            visMeter.text = "Light level: " + Player.lightLevel;
        }

        void CalcLights() {
            int lightLevel = 0;

            foreach(StealthLight light in Global.lights) {
                if(light.sees(player)) {
                    light.seesPlayer = true;

                    float distToLight = (light.transform.position - transform.position).magnitude;
                    int lightIndex;

                    if(distToLight < light.rNear) {
                        lightIndex = 3;
                    } else if(distToLight < light.rMid) {
                        lightIndex = 2;
                    } else if(distToLight < light.rFar) {
                        lightIndex = 1;
                    } else {
                        lightIndex = 0;
                    }

                    lightLevel = lightIndex > lightLevel ? lightIndex : lightLevel;
                } else {
                    light.seesPlayer = false;
                }
            }

            Player.lightLevel = lightLevel;
        }

        //void FindLights() {
        //    GameObject[] allLights = GameObject.FindGameObjectsWithTag("Light");
        //    List<GameObject> lights = new List<GameObject>();
        //    foreach(GameObject light in allLights) {
        //        if(this.sees(light)) {
        //            lights.Add(light);
        //        }
        //    }

        //    vis = 0f;

        //    if(lights.Count == 0) {
        //        return;
        //    }

        //    foreach(GameObject lightObj in lights) {
        //        float distToLight = (lightObj.transform.position - transform.position).magnitude;
        //        Light light = lightObj.GetComponent<Light>();

        //        if(distToLight < light.range) {
        //            if(distToLight == 0) {
        //                vis += 1;
        //            } else {
        //                vis += (1 - (distToLight / light.range));
        //            }
        //        }
        //    }
        //}
    }
}