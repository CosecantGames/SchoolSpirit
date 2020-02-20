using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : AIRoutine {
    private void Awake() {
    }

    public void Run(float turnSpeed = 8f, float turnAngle = 115f, float timeout = 8f) {
        Run(Routine(turnSpeed, turnAngle, timeout));
    }

    public IEnumerator Routine(float turnSpeed = 8f, float turnAngle = 115f, float timeout = Mathf.Infinity) {
        isRunning = true;

        float timer = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion leftRotation = startRotation;
        Quaternion rightRotation = startRotation;

        while(timer < timeout) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Global.Plr.transform.position - transform.position, transform.up), Time.deltaTime * turnSpeed);

            if(forceBreak) { break; }

            yield return null;
        }

        isRunning = false;
    }
}
