using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorHandle : MonoBehaviour {
    public void Use() {
        //If this threw an error, it's probably because it's not on a door handle on a door w/ OpenDoor script
        GetComponentInParent<OpenDoor>().Use();
    }
}
