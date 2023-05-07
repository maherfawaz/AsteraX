using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {
    [Header( "Inscribed" )]
    public float      speed           = 10;
    public GameObject bulletPrefab;
    public float      bulletSpeed     = 20;
    public float      aimDistFromShip = 2;
    public Transform  aimingTrans;
    public float      shotsPerSecond  = 4;
    public float      rotSpeed        = 180;

    [Header( "Dynamic" )]
    [SerializeField] // Makes this private field visible (and editable) in the Inspector
    [XnTools.ReadOnly] // This is a custom Attribute that grays out gameState so that it can't be edited
    private eGameState gameState = eGameState.idle;
    [SerializeField] [XnTools.ReadOnly] // These can also be side-by-side
    private Vector3 vel;

    public enum eGameState { idle, playing, won, lost };

    Rigidbody rigid;

    // __________ You do NOT need to edit this method __________
    void Start() {
        // Get a reference to the Rigidbody component
        rigid = GetComponent<Rigidbody>();
        // Set the initial gameState
        gameState = eGameState.playing;
    }

    // __________ You do NOT need to edit this method __________
    void Update() {
        // Aim the ship toward the mouse
        AimAtTheMouse();
        // Shoot in the direction of the mouse when the player clicks
        FireWhenReady();
        // See if all the asteroids have been destroyed
        TestForGameWon();
    }

    // vvvvvv------ #1 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// Use the Input Axes "Horizontal" and "Vertical" to move the ship. This
    ///  allows WASD, Arrow Keys, or the left stick on a controller to move
    ///  the ship.
    /// This is almost exactly what you did for movement in SpaceSHMUP,
    ///  except that you don't need to rotate the ship this time.
    /// </summary>
    void FixedUpdate() {
        // Pull input from the InputManager "Horizontal" and "Vertical" Axes
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Assign the h and v floats to the x and y values of the existing Vector3 vel
        vel.x = h;
        vel.y = v;

        // If the magnitude of vel is larger than 1, Normalize() vel.
        if (vel.magnitude > 1) vel.Normalize();

        // Multiply vel by speed so that the ship moves at a decent rate
        vel = vel * speed;

        // Set the Rigidbody rigid's velocity to vel
        rigid.velocity = vel;

    }
    // Now the ship should move when you use WASD or arrow keys. Please test it!
    // ^^^^^^------ #1 – You DO need to edit this method ------^^^^^^


    // vvvvvv------ #2 – You DO need to edit this method ------vvvvvv
    Vector3 mousePos3D;
    /// <summary>
    /// Get the position of the mouse in 3D coordinates and then point the ship
    ///  at those coordinates.
    /// You did something very similar to this to get the 3D mouse position in
    ///  Apple Picker.
    /// </summary>
    void AimAtTheMouse() {
        // Get the mouse position in 2D screen coordinates and assign it to
        //  a new Vector3 mousePosScreen
        Vector3 mousePosScreen = Input.mousePosition;

        // Set the mousePosScreen.z to -Camera.main.transform.position.z (i.e., 10)
        //  to account for the z position of the camera
        mousePosScreen.z = -Camera.main.transform.position.z;

        // Use ScreenToWorldPoint to translate mousePosScreen to world coordinates
        //  and assign the result to the existing field mousePos3D
        mousePos3D = Camera.main.ScreenToWorldPoint(mousePosScreen);

        // Aim the ship at the mouse pointer using transform.LookAt();
        //  Use the transform.LookAt(Vector3 worldPosition, Vector3 worldUp) version
        //  In this case, we want worldUp to be Vector3.back so that the top of the
        //  ship points back toward the camera.
        transform.LookAt(mousePos3D, Vector3.back);
    }
    // Now the ship should aim at the mouse correctly while flying.
    // When you test it, make sure to move both the ship and the mouse.
    // ^^^^^^------ #2 – You DO need to edit this method ------^^^^^^


    // vvvvvv------ #3 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// Fire a Bullet in the direction of mousePos3D every time the player
    ///  taps down MouseButton 0.
    /// </summary>
    void FireWhenReady() {
        // If either the player clicked MouseButton 0 or pressed KeyCode.Space down...
        if ( Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) ) {
            // If they did click the mouse, then we need to fire
            // Instantiate a bulletPrefab and store the result in a new GameObject bullGO
            GameObject bullGO = Instantiate<GameObject>(bulletPrefab);

            // Set the position of bullGO to the same position as this GameObject
            bullGO.transform.position = transform.position;

            // Create a new Vector3 deltaToMouse that is the distance from this
            //  GameObject to the mousePos3D. Remember that "A minus B looks at A"
            Vector3 deltaToMouse = mousePos3D - transform.position;

            // Set deltaToMouse to the velocity you want the bullet to have. In
            //  other words, make it point in the direction of deltaToMouse with
            //  a magnitude of bulletSpeed
            if (deltaToMouse.magnitude > 1) deltaToMouse.Normalize();
            deltaToMouse = deltaToMouse * bulletSpeed;
            // Assign this velocity to the velocity of the Rigidbody component of bullGO
            bullGO.GetComponent<Rigidbody>().velocity = deltaToMouse;
        }
    }
    // The ship should now fire Bullets whenever you click the main mouse button.
    // Test that the Bullets always fire in the direction of the mouse, no matter
    //  where the ship or mouse are.
    // The Bullets and PlayerShip can go off screen now, so after this,
    //  move to the ScreenWrap script. (You will come back to PlayerShip later.)
    // ^^^^^^------ #3 – You DO need to edit this method ------^^^^^^


    // vvvvvv------ #4 – You DO need to edit this method ------vvvvvv
    // WAIT!!! – Don't update this until you have finished LaserPointer
    void TestForGameWon() {
        // If the gameState is playing, and there are no Asteroids in the ASTEROIDS List...
        if ( (gameState == eGameState.playing) && (Asteroid.ASTEROIDS.Count == 0) ) {

            // Invoke the RestartLevel method in two seconds. Be sure that you spell the method name correctly!
            Invoke("RestartLevel", 2);

            // Set the gameState to won
            gameState = eGameState.won;
        }
    }
    // ^^^^^^------ #4 – You DO need to edit this method ------^^^^^^


    // vvvvvv------ #5 – You DO need to edit this method ------vvvvvv
    /// <summary>
    /// If this GameObject's Collider hit an Asteroid, the game should end.
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter( Collision coll ) {
        // If coll.gameObject has an Asteroid component, then we hit an Asteroid
        if (coll.gameObject.GetComponent<Asteroid>()) {

            // Disable this MonoBehaviour by setting enabled = false. This prevents
            //  Update, FixedUpdate, etc. from being called but stil allows methods
            //  to be Invoked.
            enabled = false;

            // If the gameState is still playing (i.e., we haven't yet lost)
            if (gameState == eGameState.playing) {
                // Invoke the RestartLevel method in 1 second
                Invoke("RestartLevel", 1);

                // Set gameState to lost
                gameState = eGameState.lost;

                // The following line sends an Explode message to all scripts on
                //  this GameObject but does not require anyone to be listening.
                //  You do NOT need to edit the following line.
                SendMessage("Explode", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    // YOU SHOULD NOW BE DONE. YAY!!!!!
    // Now, try playing the game and consider adding some enhancements. :)
    // Please send any feedback you have to me at jeremy (at) prototools.net
    // ^^^^^^------ #5 – You DO need to edit this method ------^^^^^^


    // __________ You do NOT need to edit this method __________
    /// <summary>
    /// Restart the level. You do NOT need to edit this script.
    /// </summary>
    void RestartLevel() {
        // Reset Time.timeScale and Time.fixedDeltaTime to their default values
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        // Reloat the 0th scene in the Build Settings.
        SceneManager.LoadScene( 0 );
    }
}
