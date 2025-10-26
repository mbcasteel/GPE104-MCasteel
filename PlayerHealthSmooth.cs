using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSmooth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public Image frontFill; // green
    public Image backFill;  // red trail
    public Gradient colorGradient;

    [Header("Animation Settings")]
    public float smoothSpeed = 5f;      // bar smooth speed
    public float trailSpeed = 2f;       // lag speed for red trail

    private float frontRatio;
    private float backRatio;

    void Start()
    {
        currentHealth = maxHealth;
        frontRatio = backRatio = 1f;
        UpdateHealthUI(true);
    }

    void Update()
    {
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI(bool instant = false)
    {
        float targetRatio = (float)currentHealth / maxHealth;

        if (instant)
        {
            frontRatio = backRatio = targetRatio;
        }
        else
        {
            // Green front bar updates quickly
            frontRatio = Mathf.Lerp(frontRatio, targetRatio, Time.deltaTime * smoothSpeed);

            // Red back bar lags behind when taking damage
            if (backRatio > targetRatio)
                backRatio = Mathf.Lerp(backRatio, targetRatio, Time.deltaTime * trailSpeed);
            else
                backRatio = targetRatio;
        }

        if (frontFill)
        {
            frontFill.fillAmount = frontRatio;
            frontFill.color = colorGradient.Evaluate(frontRatio);
        }
        if (backFill)
        {
            backFill.fillAmount = backRatio;
            backFill.color = new Color(1f, 0f, 0f, 0.8f); // semi-transparent red
        }
    }
}
