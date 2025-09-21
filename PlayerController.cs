using Unity.VisualScripting;
using UnityEngine;
// Add the correct using directive for PlayerPawn if it exists in another namespace
// using YourGameNamespace; // Uncomment and replace with the actual namespace if needed

public class PlayerController : Controller
{
    // If PlayerPawn is defined elsewhere, ensure the correct using directive is present.
    // If not, you need to define the PlayerPawn class.
    public new Pawn pawn; // Reference to the Pawn
    public Vector3 myVector = new(2, 4, 12);
    public float speed = 20f; //Speed of movement
    private Transform tf;    // A variable to hold the Transform component

    void Start()
    {
        // Get the Transform component
        tf = GetComponent<Transform>();
        tf.position += (GetComponent<Transform>().TransformDirection(new Vector3(1, 1, 0)) * speed);
        tf.Translate(Vector3.up, Space.World); // Move up in world space
        tf.Translate(Vector3.up, Space.Self); // Move up in local space
    }

    void Update()
    {
        if (pawn == null) return; // Prevents null reference errors

        // --- Local Space Inputs ---
        if (Input.GetKey(KeyCode.W))
        {
            // Check if Left or Right shift is held for turbo
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                pawn.MoveTurboForward();
            else
                pawn.MoveForward();
        }

        if (Input.GetKey(KeyCode.S))
        {
            pawn.MoveBackward();
        }

        if (Input.GetKey(KeyCode.A))
        {
            pawn.RotateLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            pawn.RotateRight();
        }

        // --- World Space Inputs ---
        if (Input.GetKey(KeyCode.UpArrow))
            pawn.MoveUpWorld();

        if (Input.GetKey(KeyCode.DownArrow))
            pawn.MoveDownWorld();

        if (Input.GetKey(KeyCode.LeftArrow))
            pawn.MoveLeftWorld();

        if (Input.GetKey(KeyCode.RightArrow))
            pawn.MoveRightWorld();

        // --- Random Teleport using T key ---
        if (Input.GetKeyDown(KeyCode.T))
            pawn.TeleportRandom();
    }

}
        
        
    
        
