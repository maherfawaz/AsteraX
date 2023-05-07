using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWrapDir { none, x, y };

public class ScreenWrap : MonoBehaviour
{
    // These are based on the "Full HD (1920x1080)" Game Pane size
    static float halfWidth = 16;
    static float halfHeight = 9;

    [Header( "Inscribed" )]
    public int wrapLimit = 3;

    [Header( "Dynamic" )]
    public int wrapCount = 0;
    [SerializeField][XnTools.ReadOnly]
    protected eWrapDir  wrapDir;

    protected Rigidbody rigid;

    // __________ You do NOT need to edit this method __________
    protected virtual void Awake() {
        rigid = GetComponent<Rigidbody>();
    }


    // __________ You do NOT need to edit this method __________
    /// <summary>
    /// This method will be overridden in Bullet so that Bullet can handle its
    ///  BulletTrail child before actually wrapping.
    /// </summary>
    protected virtual void FixedUpdate() {
        // Determine whether this GameObject should wrap
        ShouldThisWrap(); // You do not need to change this line

        // Check whether this GameObject should be destroyed because it has wrapped too much
        CheckWrapLimit();

        // Call WRAP_THIS to actually wrap this Transform to the other side of
        //  the screen. Doesn't do anything if wrapDir == wrapDir.none .
        WrapThis(); // You do not need to change this line
    }


    // vvvvvv------ #1 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// This method determines whether or not this GameObject should wrap around
    ///  the screen based on its position and velocity.
    /// This method does NOT actually wrap the GameObject to the other side of the
    ///  screen, it only determines whether this needs to happen. This separation
    ///  is used by the Bullet script, as you'll see when you get there.
    /// </summary>
    /// <returns>The direction in which this should wrap (or none)</returns>
    protected void ShouldThisWrap() {
        // These three values will help you determine whether this Transform should screen wrap
        // You do not need to edit the following three lines
        wrapDir = eWrapDir.none;
        Vector3 pos = transform.position;
        Vector3 dir = rigid.velocity.normalized;

        // If the absolute value (i.e., Mathf.Abs()) of the x value of position is
        //  greater than halfWidth, then pos is off screen in the x direction
        if (Mathf.Abs(pos.x) > halfWidth) {
            // However, if pos.x and dir.x are in opposite directions, then the
            //  GameObject is already moving back on screen
            // If pos.x times dir.x are greater than 0, then the GameObject is
            //  moving further off screen and should be wrapped
            if ((pos.x * dir.x) > 0) {
                // Set wrapDir to eWrapDir.x
                wrapDir = eWrapDir.x;
            }
        }

        // Do the same thing for the y direction that you just did for x.
        // The values to use now are: pos.y, halfHeight, dir.y, and eWrapDir.y
        if (Mathf.Abs(pos.y) > halfHeight) { 
            if ((pos.y * dir.y) > 0) {
                wrapDir = eWrapDir.y;
            }
        }


    }
    // ^^^^^^------ #1 – You DO need to edit this method ------^^^^^^


    // vvvvvv------ #2 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// Wrap the Transform trans to the other side of the screen based on wrapDir
    /// If wrapDir == eWrapDir.none, nothing happens.
    /// </summary>
    protected void WrapThis() {
        // Store the position of this transform in a new Vector3 pos
        Vector3 pos = transform.position;

        // Switch based on wrapDir
        switch (wrapDir) {
        case eWrapDir.none:
            // In the none case, no need to wrap. Just call return.
            return;
            
        case eWrapDir.x:
            // In the x case, set pos.x to negative pos.x
            pos.x = -pos.x;
            break;
            
        case eWrapDir.y:
            // Do the same thing for the y case and pos.y
            pos.y = -pos.y;
            break;
            
        }

        // Don't forget to assign pos back to transform.position
        transform.position = pos;
    }
    // ^^^^^^------ #2 – You DO need to edit this method ------^^^^^^


    // vvvvvv------ #3 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// Check the wrap count. If this instance of ScreenWrap has a wrapLimit > 0
    ///  then we need to count the number of times it has wrapped and destroy it
    ///  when it hits that limit.
    /// </summary>
    protected void CheckWrapLimit() {
        // If wrapLimit is greater than 0...
        if (wrapLimit > 0) {

            // If the value of wrapDir is something other than none...
            if (wrapDir != eWrapDir.none) {
                // Increase the wrapCount by 1
                wrapCount = wrapCount + 1;

                // If the wrapCount is greater than or equal to the wrapLimit
                if (wrapCount >= wrapLimit) {
                    // Destroy this gameObject
                    Destroy(this.gameObject);
                }
            }
        }
    }
    // Now, the PlayerShip should wrap around the screen properly. Please test it!
    // Next, move on to the Bullet script.
    // ^^^^^^------ #3 – You DO need to edit this method ------^^^^^^

}
