using UnityEngine;

/// <summary>
/// The BulletTrail will destroy itself shortly after it is detached from the Bullet
/// </summary>
[RequireComponent(typeof(TrailRenderer))]
public class BulletTrail : MonoBehaviour {


    // vvvvvv------ #1 – You DO need to edit this method ------vvvvvv
    public void CountDownToDestruction() {
        // Get a reference to the TrailRenderer Component
        //  and assign it to a new TrailRenderer tr
        TrailRenderer tr = GetComponent<TrailRenderer>();

        // Invoke the method "SelfDestruct" in tr.time seconds (i.e., the number
        //  of seconds it will take for the TrailRenderer to complete animating)
        Invoke("SelfDestruct", tr.time);
        // ^ Make sure that you spelled "SelfDestruct" correctly on the line above
    }
    // Next, let's get the Asteroids working!
    // ^^^^^^------ #1 – You DO need to edit this method ------^^^^^^


    // __________ You do NOT need to edit this method __________
    void SelfDestruct() {
        Destroy( gameObject );
    }
}
