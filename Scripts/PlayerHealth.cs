using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    private bool isDead = false;

    [Header("References")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (audioSource && hitSound)
            audioSource.PlayOneShot(hitSound);

        Debug.Log($"Player took {damage} damage (Health: {currentHealth})");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player has died!");

        if (audioSource && deathSound)
            audioSource.PlayOneShot(deathSound);

        // Notify GameManager to handle lives/respawn
        if (GameManager.instance != null)
            GameManager.instance.HandlePlayerDeath(GameManager.instance.playerController);

        // Disable player control while dead
        var controllerPlayer = GetComponent<ControllerPlayer>();
        if (controllerPlayer != null)
            controllerPlayer.enabled = false;

        var pawnSpaceship = GetComponent<PawnSpaceship>();
        if (pawnSpaceship != null)
            pawnSpaceship.enabled = false;

        // Destroy the pawn after delay (optional visual effect)
        Destroy(gameObject, 1f);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}
