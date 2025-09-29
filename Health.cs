using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Invincibility")]
    public bool isInvincible = false;
    public float invincibilityDuration = 2f; // seconds
    private float invincibilityTimer;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Count down invincibility time
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void TakeDamage(int amount, bool instantDeath)
    {
        if (isInvincible) return; // Ignore damage while invincible

        if (instantDeath)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ActivateInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has been destroyed!");

        if (CompareTag("Enemy") && GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterEnemy();
        }
        else if (CompareTag("Player") && GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }

        Destroy(gameObject);
    }
}
