using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Bullet GameObject does not have a ScreenWrap Component attached to it
///  because Bullet wrapping is complicated by the BulletTrail that will be
///  attached to each Bullet.
///  
/// In previous implementations of this game, I always had issues with the
///  Bullet TrailRenderer drawing a line all the way across the screen whenever
///  a Bullet wrapped to the other side of the screen. This AsteraX uses a separate
///  GameObject for the TrailRenderer, and every time the Bullet wraps to the
///  other side of the screen, it leaves its current BulletTrail child where it
///  was and instantiates a new BulletTrail child after the wrap.
///
/// This could cause an issue where random BulletTrails are left in the Scene,
///  so when this Bullet is destroyed, the OnDestroy() method below cleans up
///  all of the BulletTrails that were ever Instantiated by this Bullet.
///
/// Bullet extends ScreenWrap so that it can take advantage of all the ScreenWrap
///  methods while overriding ScreenWrap.FixedUpdate() to manage the BulletTrail.
/// </summary>
public class Bullet : ScreenWrap {
    public GameObject bulletTrailPrefab;

    private Transform currentTrail = null;

    // The following are inherited from ScreenWrap:
    // public int wrapLimit = 3;
    // public int wrapCount = 0;
    // protected eWrapDir  wrapDir;
    // protected Rigidbody rigid;

    // __________ You do NOT need to edit this method __________
    protected override void Awake() {
        base.Awake();  // Call ScreenWrap.Awake(), which sets rigid
        AddNewTrail(); // This call to AddNewTrail Instantiates the original BulletTrail
    }


    // vvvvvv------ #1 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// This Instantiates a new bulletTrailPrefab, makes it a child of this
    ///  transform, and assigns it to currentTrail.
    /// </summary>
    void AddNewTrail() {
        // Instantiate a new bulletTrailPrefab as a child of this transform, and
        //  assign it to a new GameObject newTrailGO
        GameObject newTrailGO = Instantiate<GameObject>(bulletTrailPrefab);
        newTrailGO.transform.SetParent(transform);
        // Make sure that the localPosition of the newTrailGO transform is [0,0,0]
        newTrailGO.transform.localPosition = Vector3.zero;

        // Assign the transform of newTrailGO to the field currentTrail.
        currentTrail = newTrailGO.transform;
    }
    // ^^^^^^------ #1 – You DO need to edit this method ------^^^^^^


    // vvvvvv------ #2 – You DO need to edit this method ------vvvvvv
    protected override void FixedUpdate() {
        // Call ShouldThisWrap(), which is inherited from ScreenWrap. This will
        //  automatically assign the wrapDir value
        ShouldThisWrap();

        // If the wrapDir is ScreenWrap.eWrapDir.none, just return. No need to do anything more.
        if (wrapDir == eWrapDir.none) return;

        // We DO need to wrap, but before we do so, we should leave currentTrail behind us
        // Set the parent of currentTrail to null (making it no longer a child of this GameObject)
        currentTrail.transform.SetParent(null);


        // Call CheckWrapLimit() and WrapThis() (both inherited from ScreenWrap)
        CheckWrapLimit();
        WrapThis();
        // Finally, call AddNewTrail() to Instantiate a new BulletTrail
        //  and assign it to currentTrail
        AddNewTrail();
    }
    // Next, please move on to BulletTrail
    // ^^^^^^------ #2 – You DO need to edit this method ------^^^^^^

}
