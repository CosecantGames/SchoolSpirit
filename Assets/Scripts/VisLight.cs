using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class VisLight : MonoBehaviour {
    public bool useFlatDist = true;

    public float distToPlr;
    public new Light light;

    public bool seesPlayer;

    public float rNear = 3f;
    public float rMid = 8f;
    public float rFar = 12f;

    public bool useFloorBelow = false;
    public Vector3 floorBelow;

    public Vector3 anchorPoint;

    // Start is called before the first frame update
    void Start() {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update() {
        seesPlayer = this.sees(Plr);
    }

    public int PlayerLightingIndex() {
        if(useFlatDist) {
            distToPlr = transform.position.flatDistTo(Plr.transform.position);
        } else {
            distToPlr = (transform.position - Plr.transform.position).magnitude;
        }

        if(distToPlr < rNear) {
            return 3;
        } else if(distToPlr < rMid) {
            return 2;
        } else if(distToPlr < rFar) {
            return 1;
        } else {
            return 0;
        }
    }

    private void OnDrawGizmosSelected() {
        anchorPoint = useFloorBelow ? floorBelow : transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(anchorPoint, rFar);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(anchorPoint, rMid);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(anchorPoint, rNear);
    }

    private void OnValidate() {
        FindFloorBelow();
        rFar = GetComponent<Light>().range;
    }

    public void FindFloorBelow() {
        Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit);
        floorBelow = hit.point;
        floorBelow.y += 0.1f;
    }
}
