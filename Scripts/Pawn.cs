using UnityEngine;

public class Pawn : MonoBehaviour
{
    protected Transform tf;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;

    [Header("Turbo Settings")]
    public float turboSpeedMultiplier = 2.5f;
    public float turboDuration = 1.5f;
    public float turboCooldown = 3f;
    private bool isTurboActive = false;
    private bool canTurbo = true;

    [Header("Teleport Settings")]
    public Vector2 teleportBoundsMin = new Vector2(-10f, -5f);
    public Vector2 teleportBoundsMax = new Vector2(10f, 5f);

    [Header("Stats")]
    public int lives = 3;

    protected virtual void Start()
    {
        tf = transform;
    }

    protected virtual void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //  WASD Controls (rotation-based)
        if (Input.GetKey(KeyCode.W))
            MoveForward();
        if (Input.GetKey(KeyCode.S))
            MoveBackward();
        if (Input.GetKey(KeyCode.A))
            RotateLeft();
        if (Input.GetKey(KeyCode.D))
            RotateRight();

        // Arrow Key Controls
        if (Input.GetKey(KeyCode.UpArrow))
            MoveForward();  // same as W
        if (Input.GetKey(KeyCode.DownArrow))
            MoveBackward(); // same as S
        if (Input.GetKey(KeyCode.LeftArrow))
            StrafeLeft();   // new world-space movement
        if (Input.GetKey(KeyCode.RightArrow))
            StrafeRight();  // new world-space movement

        // Turbo Boost
        if (Input.GetKeyDown(KeyCode.LeftShift))
            TryActivateTurbo();

        // Teleport
        if (Input.GetKeyDown(KeyCode.T))
            TeleportRandom();
    }

    //  Movement (relative to facing)
    public virtual void MoveForward()
    {
        tf.position += moveSpeed * Time.deltaTime * tf.up;
    }

    public virtual void MoveBackward()
    {
        tf.position -= moveSpeed * Time.deltaTime * tf.up;
    }

    // Rotation (local)
    public virtual void RotateLeft()
    {
        tf.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public virtual void RotateRight()
    {
        tf.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }

    // Strafing (world-space)
    public virtual void StrafeLeft()
    {
        tf.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    public virtual void StrafeRight()
    {
        tf.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    // Teleport
    public virtual void TeleportRandom()
    {
        float randomX = Random.Range(teleportBoundsMin.x, teleportBoundsMax.x);
        float randomY = Random.Range(teleportBoundsMin.y, teleportBoundsMax.y);
        tf.position = new Vector3(randomX, randomY, tf.position.z);
        Debug.Log($"{gameObject.name} teleported to ({randomX:F2}, {randomY:F2})");
    }

    // Turbo Boost
    private void TryActivateTurbo()
    {
        if (!canTurbo) return;
        StartCoroutine(TurboRoutine());
    }

    private System.Collections.IEnumerator TurboRoutine()
    {
        canTurbo = false;
        isTurboActive = true;
        float originalSpeed = moveSpeed;

        moveSpeed *= turboSpeedMultiplier;
        Debug.Log("Turbo boost activated!");

        yield return new WaitForSeconds(turboDuration);

        moveSpeed = originalSpeed;
        isTurboActive = false;
        Debug.Log("Turbo boost ended.");

        yield return new WaitForSeconds(turboCooldown);
        canTurbo = true;
        Debug.Log("Turbo boost ready again.");
    }
}
