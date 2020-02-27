using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class VisLight : MonoBehaviour {
    public bool useFlatDist = true;

    public float distToPlr;
    public new Light light;

    public bool seesPlayer;

    public float lowLightRadius = 15f;
    public float midLightRadius = 8f;
    public float highLightRadius = 5f;

    // Start is called before the first frame update
    void Start() {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update() {
        seesPlayer = this.sees(Plr);
    }

    public int PlayerLightingIndex() {
        float visOnPlayer = 0f;

        if(useFlatDist) {
            distToPlr = transform.position.flatDistTo(Plr.transform.position);
        } else {
            distToPlr = (transform.position - Plr.transform.position).magnitude;
        }

        if(distToPlr < highLightRadius) {
            return 3;
        } else if(distToPlr < midLightRadius) {
            return 2;
        } else if(distToPlr < lowLightRadius) {
            return 1;
        } else {
            return 0;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lowLightRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, midLightRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, highLightRadius);
    }
}
