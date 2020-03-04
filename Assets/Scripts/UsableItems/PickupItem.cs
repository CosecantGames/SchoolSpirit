using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour {
    public Vector3 invPosition = Vector3.zero;
    public Vector3 invRotation = Vector3.zero;

    public void Use() {
        Player.Player.Inv.Add(gameObject);
    }
}
