using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : AIRoutine {
    private void Awake() {
    }

    public void Run(float turnSpeed = 7.5f, float turnAngle = 125f, float timeout = Mathf.Infinity) {
        Run(Routine(turnSpeed, turnAngle, timeout));
    }

    public IEnumerator Routine(float turnSpeed = 7.5f, float turnAngle = 125f, float timeout = Mathf.Infinity) {
        isRunning = true;

        float timer = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion leftRotation = Quaternion.Euler(0f, -turnAngle, 0f) * startRotation;
        Quaternion rightRotation = Quaternion.Euler(0f, turnAngle, 0f) * startRotation;
        Quaternion target = leftRotation;
        Quaternion endTarget = leftRotation;

        while(timer < timeout) {
            timer += Time.deltaTime;

            if(target == startRotation) {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, turnSpeed);
            } else {
                transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * turnSpeed);
            }

            float angleToTarget = Quaternion.Angle(transform.rotation, target);

            if(angleToTarget <= 2) {
                if(target == leftRotation) {
                    target = startRotation;
                    endTarget = rightRotation;
                } else if(target == rightRotation) {
                    target = startRotation;
                    endTarget = leftRotation;
                } else {
                    target = endTarget;
                }
            }

            if(forceBreak) { break; }

            yield return null;
        }

        isRunning = false;
    }
}
