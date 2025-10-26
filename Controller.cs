using UnityEngine;

public class Controller : MonoBehaviour
{
    public Pawn pawn; // Drag your Pawn GameObject here in the Inspector

    void Update()
    {
        if (pawn == null)
            return;

        // W/S = Move forward/back
        if (Input.GetKey(KeyCode.W))
            pawn.MoveForward();

        if (Input.GetKey(KeyCode.S))
            pawn.MoveBackward();

        // A/D = Rotate left/right
        if (Input.GetKey(KeyCode.A))
            pawn.RotateLeft();

        if (Input.GetKey(KeyCode.D))
            pawn.RotateRight();
    }
}
