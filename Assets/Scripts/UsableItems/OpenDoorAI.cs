using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorAI : MonoBehaviour {
    public OpenDoor door;

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log(other.name + " entered door area");
        if(door.isOpen == false && door.IsLocked == false && other.tag == "Enemy") {
            door.isOpen = true;
            door.autoClose = true;
        }
    }

    //private void OnTriggerStay(Collider other) {
    //    if(other.tag == "Enemy") {
    //        door.isOpen = true;
    //    }
    //}

    private void OnTriggerExit(Collider other) {
        //Debug.Log(other.name + " exited door area");
        if(door.autoClose) {
            door.isOpen = false;
            door.autoClose = false;
        }
    }
}
