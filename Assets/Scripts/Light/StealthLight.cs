using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthLight : MonoBehaviour {
    public float rNear = 3f;
    public float rMid = 8f;
    public float rFar = 12f;

    public Light nearLight;
    public Light midLight;
    public Light farLight;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rNear);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rMid);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rFar);
    }

    private void OnValidate() {
        nearLight.range = rNear;
        midLight.range = rMid;
        farLight.range = rFar;
    }
}
