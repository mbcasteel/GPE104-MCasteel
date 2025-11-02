using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PawnSpaceship : Pawn
{
    [Header("Movement Settings")]
    public float thrustPower = 6f;
    public float maxSpeed = 8f;
    public float rotationSpeed = 220f;
    public float driftDampening = 0.985f;

    [Header("Respawn Protection")]
    public float invincibilityDuration = 3f;   // seconds of safety
    public float blinkInterval = 0.2f;         // how fast to flash sprite
    public GameObject shieldVisual;            // optional: assign glow FX

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprite;
    private bool isInvincible = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.linearDamping = 0;
        rb.angularDamping = 0;
    }

    public void Move(Vector3 input)
    {
        // Rotate (A/D)
        float rotation = -input.x * rotationSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation + rotation);

        // Thrust (W)
        if (input.y > 0)
        {
            Vector2 thrustForce = transform.up * input.y * thrustPower * Time.deltaTime * 60f;
            rb.AddForce(thrustForce);
        }

        // Damp velocity & cap speed
        rb.linearVelocity *= driftDampening;
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    // Called right after respawn by GameManager
    public void ActivateInvincibility()
    {
        if (!isInvincible)
            StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        col.enabled = false; // Disable collisions with meteors

        if (shieldVisual) shieldVisual.SetActive(true);

        float timer = 0f;
        bool visible = true;

        while (timer < invincibilityDuration)
        {
            timer += blinkInterval;
            visible = !visible;
            if (sprite) sprite.enabled = visible;
            yield return new WaitForSeconds(blinkInterval);
        }

        // End protection
        if (sprite) sprite.enabled = true;
        col.enabled = true;
        if (shieldVisual) shieldVisual.SetActive(false);

        isInvincible = false;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
