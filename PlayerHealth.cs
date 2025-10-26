using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public Image healthFill;            // Green main fill
    public Gradient healthGradient;     // Green → Yellow → Red

    [Header("Animation Settings")]
    public float smoothSpeed = 5f;      // How fast the bar catches up

    private float displayedHealthRatio; // Used for smooth animation

    void Start()
    {
        currentHealth = maxHealth;
        displayedHealthRatio = 1f;
        UpdateHealthUI(true);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        UpdateHealthUI();
        if (currentHealth <= 0)
            Die();

        SoundManager.instance.PlaySFX(SoundManager.instance.hitSound);

    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI(bool instant = false)
    {
        if (healthFill == null) return;

        float targetRatio = (float)currentHealth / maxHealth;

        if (instant)
            displayedHealthRatio = targetRatio;
        else
            displayedHealthRatio = Mathf.Lerp(displayedHealthRatio, targetRatio, Time.deltaTime * smoothSpeed);

        healthFill.fillAmount = displayedHealthRatio;
        healthFill.color = healthGradient.Evaluate(displayedHealthRatio);
    }

    private void Die()
    {
        Debug.Log("💀 Player Died!");
        // Insert death logic / respawn
    }
}
