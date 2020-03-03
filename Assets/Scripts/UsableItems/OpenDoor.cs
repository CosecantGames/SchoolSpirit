using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {
    public enum OpenAxis {
        X,
        Y,
        Z
    }

    public bool reverseDirection = false;
    public bool isOpen = false;
    public float openAngle = 100;
    public float openSpeed = 5f;
    public OpenAxis openAxis = OpenAxis.Y;
    Quaternion closeRot;
    Quaternion openRot;
    Quaternion targetRot;

    private void Awake() {
        closeRot = transform.rotation;
        openRot = closeRot;

        switch(openAxis) {
            case OpenAxis.X:
                openRot.eulerAngles -= new Vector3(reverseDirection ? openAngle * -1 : openAngle, 0f, 0f);
                break;
            case OpenAxis.Y:
                openRot.eulerAngles -= new Vector3(0f, reverseDirection ? openAngle * -1 : openAngle, 0f);
                break;
            case OpenAxis.Z:
                openRot.eulerAngles -= new Vector3(0f, 0f, reverseDirection ? openAngle * -1 : openAngle);
                break;
        }
    }

    private void Update() {
        targetRot = isOpen ? openRot : closeRot;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, openSpeed);
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Hit " + collision.gameObject.name);
    }

    public void Use() {
        Debug.Log("Used door");
        isOpen = !isOpen;
    }
}
