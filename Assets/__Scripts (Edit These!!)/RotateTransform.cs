using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotateTransform : MonoBehaviour {
    public Vector3 angularVelocity = new Vector3( -0.02f, 0.04f, -0.01f );


    // A very simple method to cause the background planet to rotate slowly
    // __________ You do NOT need to edit this method __________
    private void Awake() {
        // Set the initial rotational velocity
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.angularVelocity = angularVelocity;
    }
}
