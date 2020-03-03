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
            //visMeter.text = "Light level: " + Player.lightLevel + "\nEnemy A Proximity: " + enemy.playerProximity + "\nA's Awareness: " + enemy.awareness + "\nEnemy B Proximity: " + enemyB.playerProximity + "\nB's Awareness: " + enemyB.awareness;
        }

        void CalcLights() {
            int lightLevel = 0;

            foreach(VisLight light in Global.lightScripts) {
                if(light.seesPlayer) {
                    int lightIndex = light.PlayerLightingIndex();
                    lightLevel = lightIndex > lightLevel ? lightIndex : lightLevel;
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