using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpenDoor : MonoBehaviour {
    public enum OpenAxis {
        X,
        Y,
        Z
    }

    [Header("Options")]
    public bool reverseDirection = false;
    public OpenAxis openAxis = OpenAxis.Y;
    public float openAngle = 100;
    public float openSpeed = 5f;

    [Space(8)]
    public bool isOpen = false;
    //locked is for setting its initial value and testing w/ inspector. isLocked should be used during runtime.
    public bool locked = false;
    public GameObject key;

    private bool isLocked = false;
    public bool IsLocked {
        get { return isLocked; }
        set {
            isLocked = value;
            SetObstacle();
        }
    }

    public float closeTimer = 0f;
    public bool autoClose = false;

    Quaternion closeRot;
    Quaternion openRot;
    Quaternion targetRot;

    private void Awake() {
        closeRot = transform.rotation;
        openRot = closeRot;

        IsLocked = locked;
        GetComponent<NavMeshObstacle>().enabled = IsLocked;

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

        //if(autoClose) {
        //    if(isOpen) {
        //        closeTimer -= Time.deltaTime;
        //    }

        //    if(closeTimer <= 0) {
        //        isOpen = false;
        //        autoClose = false;
        //    }
        //}
    }

    public void Use() {
        if(Player.Player.Inv.HeldItem == key) {
            IsLocked = !IsLocked;
        }

        if(isLocked) {
            isOpen = false;
        } else {
            isOpen = !isOpen;
        }
    }

    public void SetObstacle() {
        GetComponent<NavMeshObstacle>().enabled = IsLocked;
    }

    private void OnValidate() {
        IsLocked = locked;
    }
}
