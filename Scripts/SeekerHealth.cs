using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class SeekerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI References")]
    public Canvas worldSpaceCanvas;
    public Image healthFill;
    public float fadeDelay = 1.5f;
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (worldSpaceCanvas != null)
        {
            canvasGroup = worldSpaceCanvas.GetComponent<CanvasGroup>() ??
                          worldSpaceCanvas.gameObject.AddComponent<CanvasGroup>();

            worldSpaceCanvas.enabled = false;
            canvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        // Keep bar upright
        if (worldSpaceCanvas != null)
            worldSpaceCanvas.transform.rotation = Quaternion.identity;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log($"?? Seeker took {amount} damage. Health: {currentHealth}/{maxHealth}");

        ShowHealthBar();
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowHealthBar()
    {
        if (worldSpaceCanvas == null || healthFill == null) return;

        worldSpaceCanvas.enabled = true;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        StartCoroutine(FadeCanvas(1f, 0.15f)); // fade in quickly
    }

    private void UpdateHealthUI()
    {
        if (healthFill == null) return;

        float pct = Mathf.Clamp01((float)currentHealth / maxHealth);
        healthFill.fillAmount = pct;

        // Reset fade timer
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutAfterDelay());
    }

    private IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(fadeDelay);
        yield return FadeCanvas(0f, fadeDuration);
        worldSpaceCanvas.enabled = false;
    }

    private IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        if (canvasGroup == null) yield break;

        float startAlpha = canvasGroup.alpha;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    private void Die()
    {
        Debug.Log("?? Seeker destroyed!");
        Destroy(gameObject);
    }
}
