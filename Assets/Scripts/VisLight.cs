using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisLight : MonoBehaviour {
    public bool useFlatDist = true;

    public float distToPlr;
    public new Light light;

    public bool seesPlayer;

    //How close the player has to be to receive the minimum amount of light
    public float minLightRadius = 15f;
    //How close the player has to be to recieve the maximum amount of light
    public float maxLightRadius = 1f;

    // Start is called before the first frame update
    void Start() {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update() {
        seesPlayer = this.sees(Global.Plr);
    }

    float Map(float x, float in_min, float in_max, float out_min, float out_max) {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public float PlayerLightingIndex() {
        float visOnPlayer = 0f;

        if(useFlatDist) {
            distToPlr = transform.position.flatDistTo(Global.Plr.transform.position);
        } else {
            distToPlr = (transform.position - Global.Plr.transform.position).magnitude;
        }

        if(distToPlr > minLightRadius) {
            visOnPlayer = 0f;
        } else if(distToPlr < maxLightRadius) {
            visOnPlayer = 1f;
        } else {
            float mappedDist = Map(distToPlr, maxLightRadius, minLightRadius, 1f, 0f);
            visOnPlayer = mappedDist;
        }

        return visOnPlayer;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minLightRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxLightRadius);
    }
}
