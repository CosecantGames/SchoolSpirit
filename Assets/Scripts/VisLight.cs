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
    public Vector3 floorBelow;

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
        Gizmos.DrawWireSphere(floorBelow, lowLightRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(floorBelow, midLightRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(floorBelow, highLightRadius);
    }

    private void OnValidate() {
        Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit);
        floorBelow = hit.point;
        floorBelow.y += 0.1f;
    }
}
