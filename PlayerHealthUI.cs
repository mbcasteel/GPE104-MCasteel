using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI References")]
    public Image healthFill;
    public Gradient colorGradient;

    [Header("Target")]
    public PlayerHealth playerHealth; // your player health script

    void Start()
    {
        if (playerHealth == null)
        {
            // Automatically find player health if not assigned
            playerHealth = Object.FindFirstObjectByType<PlayerHealth>();
        }
    }

    void Update()
    {
        if (playerHealth == null || healthFill == null) return;

        float pct = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        healthFill.fillAmount = pct;
        healthFill.color = colorGradient.Evaluate(pct);
    }
}
