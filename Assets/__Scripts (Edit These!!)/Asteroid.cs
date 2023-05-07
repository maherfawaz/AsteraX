using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the last big script you'll need to do!
/// </summary>
[RequireComponent( typeof( Rigidbody ) )]
public class Asteroid : MonoBehaviour {
    static public List<Asteroid> ASTEROIDS = new List<Asteroid>();

    public int      size = 3;
    public float    speed = 15;
    public float    sizeScale = 0.5f;
    public int      numChildAsteroids = 2;


    // vvvvvv------ #1 – You DO need to edit this method ------vvvvvv
    void Start() {
        // Set the localScale to [1,1,1] times size times sizeScale. This will
        //  give the size 3 asteroids a localScale of [ 1.5, 1.5, 1.5 ]
        //  and size 1 asteroids a localScale of [ 0.5, 0.5, 0.5 ]
        transform.localScale = Vector3.one * size * sizeScale;
        

        // Next we want to randomize the initial velocity and scale it based on size.
        //  Start by assigning a Random.insideUnitCircle value to a new Vector3 vel,
        //  which ensures that vel points in a random xy direction with a z=0.
        Vector3 vel = Random.insideUnitCircle;

        // Normalize vel to set its magnitude to 1
        vel.Normalize();


        // The speed of the Asteroid should be within a range that works for its size.
        //  Create a new float randSpeed and set it to a Random.Range() value between
        //  (speed / (size+1) ) and (speed / size). For a size 3 Asteroid, this will
        //  result in a randSpeed between 15*0.25f and 15*0.33f.
        float randSpeed = Random.Range((speed / (size + 1)), (speed / size));

        // Multiply vel by randSpeed to get the final velocity for the Asteroid
        vel = vel * randSpeed;

        // Get a reference to the Rigidbody component and set its velocity to vel
        //  Note: Because of the RequireComponent at the top of this script,
        //  you know that a Rigidbody is attached, so you don't need to check
        //  to see if the result of the GetComponent() call is null.
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.velocity = vel;
        // Next, give it a little spin with Rigidbody.angularVelocity.
        //  Create a new Vector3 angularVel
        Vector3 angularVel;


        // Assign random values between -2f and 2f to each of the x, y, and z values of angularVel
        //  (This will take three separate lines or one REALLY long line.)
        angularVel.x = Random.Range(-2f, 2f);
        angularVel.y = Random.Range(-2f, 2f);
        angularVel.z = Random.Range(-2f, 2f);
        // Set the angularVelocity of the Rigidbody to angularVel
        rigid.angularVelocity = angularVel;


        // Add this Asteroid to the static public List<Asteroid> ASTEROIDS. This
        //  will allow PlayerShip to know whether all of the Asteroids have been
        //  destroyed.
        ASTEROIDS.Add(this);
    }
    // ^^^^^^------ #1 – You DO need to edit this method ------^^^^^^



    // vvvvvv------ #2 – You DO need to edit this method ------vvvvvv
    void OnDestroy() {
        /// When this Asteroid is destroyed, it needs to be removed from the ASTEROIDS List
        ASTEROIDS.Remove(this);
    }
    // ^^^^^^------ #2 – You DO need to edit this method ------^^^^^^



    // vvvvvv------ #3 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// When this Asteroid is hit by a Bullet, it should make two smaller
    ///  Asteroids and then Destroy itself. Because all of the scaling and
    ///  initial velocity setting occurs in Start() the updated size of the
    ///  new Asteroids will be set before their scaling and velocity are set.
    /// </summary>
    /// <param name="coll">The Collider that hit us</param>
    void OnCollisionEnter( Collision coll ) {
        // If the Collider that hit us is on a GameObject with a Bullet Component attached...
        if (coll.gameObject.GetComponent<Bullet>()) {
            // We are dealing with a Bullet!
            // Destroy the Bullet gameObject
            Destroy(coll.gameObject);

            // If this Asteroid's size > 1...
            if (this.size > 1) {
                // then Instantiate numChildAsteroids (i.e., 2) child Asteroids
                for (numChildAsteroids=0; numChildAsteroids<2; numChildAsteroids++) {
                    // Instantiate each child Asteroid as a copy of this gameObject
                    //  (rather than Instantiating a prefab). To do this, pass this
                    //  gameObject in as the argument to the Instantiate() call.
                    GameObject child = Instantiate<GameObject>(this.gameObject);

                    // Get the Asteroid component of the newly-Instantiated Asteroid...
                    Asteroid ast = child.GetComponent<Asteroid>();

                    // ...and set the size of that Asteroid component to be one
                    //  less than the size of this Asteroid
                    ast.size = size - 1;
                }
            }

            // Finally, Destroy this Asteroid
            Destroy(this.gameObject);
        }
    }
    // Now, you can go back to PlayerShip and finish that script!
    // ^^^^^^------ #3 – You DO need to edit this method ------^^^^^^
}
