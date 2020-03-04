using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObj : MonoBehaviour {
    public Vector3 rotation = Vector3.up;
    public float speed = 1f;

    // Update is called once per frame
    void Update() {
        transform.eulerAngles += (rotation * speed);
    }
}
