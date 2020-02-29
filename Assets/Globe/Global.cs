using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player.Player;

public class Global : MonoBehaviour {
    public static Player.Player Plr;
    public static GameObject[] lightObjs;
    public static List<Light> lights = new List<Light>();
    public static List<VisLight> lightScripts = new List<VisLight>();

    void Start() {
        Plr = GameObject.Find("player").GetComponent<Player.Player>();
        lightObjs = GameObject.FindGameObjectsWithTag("Light");

        foreach(GameObject lightObj in lightObjs) {
            lights.Add(lightObj.GetComponent<Light>());
        }

        foreach(GameObject lightObj in lightObjs) {
            lightScripts.Add(lightObj.GetComponent<VisLight>());
        }
    }

    //Thanks Arduino!
    public static float Map(float x, float in_min, float in_max, float out_min, float out_max) {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
