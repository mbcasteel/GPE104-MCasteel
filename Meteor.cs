using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Meteor : MonoBehaviour
{
    [Header("Meteor Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Drift Settings")]
    public float driftSpeedMin = 0.5f;
    public float driftSpeedMax = 2f;
    public float spinSpeedMin = -50f;
    public float spinSpeedMax = 50f;

    [Header("Health Bar UI")]
    public Canvas worldSpaceCanvas;     // assign in prefab
    public Image healthFill;            // radial or horizontal fill image
    public float fadeDelay = 1.5f;
    public float fadeDuration = 0.5f;

    [Header("Explosion & Fragmentation")]
    public GameObject explosionPrefab;  // optional
    public AudioClip explosionSound;
    public AudioSource audioSource;
    public GameObject fragmentPrefab;   // smaller meteor prefab
    public int fragmentsToSpawn = 2;
    public float fragmentScale = 0.6f;
    public float fragmentHealthFactor = 0.5f;

    private Rigidbody2D rb;
    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearDamping = 0;

        if (worldSpaceCanvas != null)
        {
            canvasGroup = worldSpaceCanvas.GetComponent<CanvasGroup>() ?? worldSpaceCanvas.gameObject.AddComponent<CanvasGroup>();
            worldSpaceCanvas.enabled = false;
            canvasGroup.alpha = 0f;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;

        // Random drift and spin
        Vector2 dir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = dir * Random.Range(driftSpeedMin, driftSpeedMax);
        rb.angularVelocity = Random.Range(spinSpeedMin, spinSpeedMax);
    }

    private void Update()
    {
        // Keep health bar upright (for world-space canvases)
        if (worldSpaceCanvas != null)
            worldSpaceCanvas.transform.rotation = Quaternion.identity;
    }

    // --- Screen Wrapping ---
    private void LateUpdate()
    {
        WrapAroundScreen();
    }

    private void WrapAroundScreen()
    {
        if (Camera.main == null) return;

        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Camera.main.aspect;

        Vector3 pos = transform.position;
        float margin = 0.5f; // small buffer so it wraps smoothly off-screen

        if (pos.x > halfWidth + margin)
            pos.x = -halfWidth - margin;
        else if (pos.x < -halfWidth - margin)
            pos.x = halfWidth + margin;

        if (pos.y > halfHeight + margin)
            pos.y = -halfHeight - margin;
        else if (pos.y < -halfHeight - margin)
            pos.y = halfHeight + margin;

        transform.position = pos;
    }


    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
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
        StartCoroutine(FadeCanvas(1f, 0.15f)); // fade in fast
    }

    private void UpdateHealthUI()
    {
        if (healthFill == null) return;

        float pct = Mathf.Clamp01((float)currentHealth / maxHealth);
        healthFill.fillAmount = pct;

        // Reset fade timer after taking damage
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
        // Explosion FX
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Explosion Sound
        SoundManager.instance.PlaySFX(SoundManager.instance.explosionSound);


        //  Spawn fragments
        SpawnFragments();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // prevent hitting invincible player
            PawnSpaceship ship = other.GetComponent<PawnSpaceship>();
            if (ship != null && ship.IsInvincible())
                return;

            playerHealth.TakeDamage(25); //  adjust damage value as needed
            Debug.Log("☄️ Meteor hit player!");

            // Optional: destroy the meteor after impact
            Die();
        }
    }

    private void SpawnFragments()
    {
        if (fragmentPrefab == null || fragmentsToSpawn <= 0) return;

        for (int i = 0; i < fragmentsToSpawn; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0f);
            GameObject frag = Instantiate(fragmentPrefab, spawnPos, Quaternion.identity);

            // Scale smaller
            frag.transform.localScale = transform.localScale * fragmentScale;

            Meteor fragMeteor = frag.GetComponent<Meteor>();
            if (fragMeteor != null)
            {
                fragMeteor.maxHealth = Mathf.Max(10, (int)(maxHealth * fragmentHealthFactor));
                fragMeteor.currentHealth = fragMeteor.maxHealth;
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log($"💥 Meteor '{name}' destroyed. Attempting to add score...");

        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(100);
            Debug.Log($"✅ Score added! New total: {GameManager.instance.playerScore}");
        }
        else
        {
            Debug.LogWarning("⚠️ GameManager.instance is null — cannot add score!");
        }
    }

}
