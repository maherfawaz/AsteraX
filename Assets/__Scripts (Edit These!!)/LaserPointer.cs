using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {
    public  Gradient     missColor, hitColor;
    private float        maxDist = 100;
    private LineRenderer line;
    private int          mask;


    // vvvvvv------ #1 – You DO need to edit this method ------vvvvvv
    private void Awake() {
        // Get a reference to the LineRenderer Component on this GameObject and assign it to line
        line = GetComponent<LineRenderer>();

        // Create a LayerMask that will only see the "Asteroid" layer by using
        //  LayerMask.GetMask() - https://docs.unity3d.com/2020.3/Documentation/ScriptReference/LayerMask.GetMask.html
        //  Assign the result to the int mask. This doesn't require a conversion
        //  because LayerMasks can implicitly conver to ints
        mask = LayerMask.GetMask();
    }
    // ^^^^^^------ #1 – You DO need to edit this method ------^^^^^^



    // vvvvvv------ #2 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// Every Update the LaserPointer draws a LineRenderer from the current position
    ///  of this GameObject to a point in the transform.forward direction. It uses
    ///  a Physics.Raycast() to determine if the line hits anything. If the line
    ///  does hit something, it turns brighter and stops at the point where it hit.
    /// </summary>
    private void Update() {
        // You do not need to edit the following three lines.
        Vector3 p0, p1; // The two endpoints of the line
        Vector3 dir;    // The direction in which the line will point
        RaycastHit hit; // A record of what the Physics.Raycast hit (or null if it missed everything)

        // Assign the world space position of this transform to the Vector3 p0
        p0 = transform.position;

        // Use transform.forward to find the Vector3 dir in which the laser should point
        dir = transform.forward;

        // Use Physics.Raycast to find the first Asteroid we hit. The version you want to use is
        //  static public bool Physics.Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
        //  https://docs.unity3d.com/2020.3/Documentation/ScriptReference/Physics.Raycast.html
        //  Remember that Physics.Raycast() returns true if it hit anything, and you need to use
        //  "out" on the hitInfo argument (declared on the 4th line of this Update method)
        //  for it to give you back the info about the hit.
        if ( Physics.Raycast(p0, dir, out hit, maxDist, mask) ) {
            // If the result of the Raycast is true, then set p1 to the point that was hit
            p1 = hit.transform.position;

            // Set the colorGradient of the line to hitColor
            line.colorGradient = hitColor;
        } else {
            // If the result of the Raycast is false, then set p1 to a point maxDist away
            //  from p0 in the direction the ship is pointing.
            p1 = p0 + (dir * maxDist);

            // Set the colorGradient of the line to missColor
            line.colorGradient = missColor;
        }

        // Finally, assign p0 and p1 to the two positions of the LineRenderer line
        line.SetPosition(0, p0);
        line.SetPosition(1, p1);
    }
    // ^^^^^^------ #2 – You DO need to edit this method ------^^^^^^
}
