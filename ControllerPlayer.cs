using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerPlayer : MonoBehaviour
{
    public PawnSpaceship pawn;
    private Vector2 moveInput;

    // Add this field to fix CS1061
    public int lives = 3;

    private void Update()
    {
        if (pawn == null)
            return;

        // Movement based on input
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);
        pawn.Move(move);
    }

    // This is called by the Input System when Move changes
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
