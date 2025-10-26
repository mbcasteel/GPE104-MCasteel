using UnityEngine;

public class SeekerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;           // Units per second
    public float rotationSpeed = 200f;     // Degrees per second
    public float stopDistance = 0.5f;      // How close it gets before stopping

    private Transform playerTarget;

    void Update()
    {
        // Verify that the GameManager and Player exist
        if (GameManager.instance == null ||
            GameManager.instance.playerController == null ||
            GameManager.instance.playerController.pawn == null)
            return;

        // Cache the player’s pawn transform
        playerTarget = GameManager.instance.playerController.pawn.transform;

        // Compute direction to the player
        Vector2 direction = (playerTarget.position - transform.position);
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            // Normalize direction
            direction.Normalize();

            // Move forward toward the player
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }

        // Smoothly rotate toward the player
        RotateTowardPlayer(direction);
    }

    private void RotateTowardPlayer(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;

        // Compute target rotation in degrees
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Smoothly rotate toward that angle
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }



}