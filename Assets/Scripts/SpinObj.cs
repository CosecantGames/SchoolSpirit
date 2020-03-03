﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObj : MonoBehaviour {
    public Vector3 rotation = Vector3.zero;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start() {
        rotation.y = 0.2f;
    }

    // Update is called once per frame
    void Update() {
        transform.eulerAngles += (rotation * speed);
    }
}
