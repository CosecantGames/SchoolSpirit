using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisLight : MonoBehaviour {
    public float distToPlr;
    public new Light light;

    // Start is called before the first frame update
    void Start() {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update() {
        distToPlr = (transform.position - Global.Plr.transform.position).magnitude;
    }
}
