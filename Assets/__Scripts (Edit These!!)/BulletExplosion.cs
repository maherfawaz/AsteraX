using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// __________ You do NOT need to edit the BulletExplosion class at all __________
public class BulletExplosion : MonoBehaviour {
    public GameObject bulletPrefab;
    public int        numBullets        = 12;
    public float      speed             = 20f;
    public float      stopDuration      = 0.5f;
    public float      postStopTimeScale = 0.25f;

    private float stopTime = -1;
    
    public void Explode() {
        // Stop movement on this GameObject
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;


        float rotPerBullet = 360f / numBullets;
        for ( int i = 0; i < numBullets; i++ ) {
            GameObject go = Instantiate<GameObject>( bulletPrefab );
            go.transform.position = transform.position;
            Rigidbody rigid = go.GetComponent<Rigidbody>();
            Vector3 dir = Quaternion.Euler(0,0,i*rotPerBullet) * Vector3.right;
            rigid.velocity = dir * Random.Range(speed/2,speed);
        }
        // Hit Stop!!! – https://www.youtube.com/watch?v=OdVkEOzdCPw
        stopTime = Time.realtimeSinceStartup;
        Time.timeScale = 0.01f;
    }

    private void Update() {
        if ( stopTime > 0 ) {
            if ( Time.realtimeSinceStartup - stopTime > stopDuration ) {
                Time.timeScale = postStopTimeScale;
                Time.fixedDeltaTime = 0.02f * postStopTimeScale;
                stopTime = -1;
                gameObject.SetActive(false);
            }
        }
    }
}
