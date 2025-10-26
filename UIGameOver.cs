using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    public Image fadePanel;                 // Black overlay
    public TextMeshProUGUI mainText;        // "GAME OVER" or countdown text
    public GameObject buttonPanel;          // Holds Restart/Quit buttons

    [Header("Audio")]
    public AudioSource audioSource;         // General SFX (e.g. gameOverBoom)
    public AudioSource heartbeatSource;     // Looping heartbeat
    public AudioClip countdownBeep;         // Short beep between countdown ticks
    public AudioClip gameOverBoom;          // Big impact sound on Game Over

    [Header("Timing Settings")]
    public float fadeDuration = 1.5f;       // Fade in/out speed
    public float countdownDelay = 1f;       // Delay between countdown numbers
    public float postGameOverDelay = 2f;    // Time before buttons show

    [Header("Heartbeat Settings")]
    public float heartbeatFadeInTime = 1.5f;
    public float heartbeatFadeOutTime = 1f;
    public float heartbeatMaxVolume = 1f;

    [Header("Visuals")]
    public float pulseSpeed = 2f;           // "GAME OVER" pulsing speed

    private Coroutine pulseRoutine;

    private void Start()
    {
        // Initialize UI invisible
        SetAlpha(fadePanel, 0);
        SetAlpha(mainText, 0);

        if (buttonPanel)
            buttonPanel.SetActive(false);

        if (heartbeatSource != null)
        {
            heartbeatSource.volume = 0f;
            heartbeatSource.loop = true;
        }
    }

    // ------------------------
    // Game Over Sequence
    // ------------------------
    public void ShowGameOver()
    {
        StopAllCoroutines();
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        mainText.text = "GAME OVER";

        // 1?? Fade in black screen + text
        yield return StartCoroutine(FadePanel(1f));
        yield return StartCoroutine(FadeText(1f));

        // 2?? Play impact sound and heartbeat
        if (audioSource && gameOverBoom)
            audioSource.PlayOneShot(gameOverBoom);
        StartCoroutine(FadeInHeartbeat());

        // 3?? Start pulsing “GAME OVER” text
        pulseRoutine = StartCoroutine(PulseText());

        // 4?? Wait before showing buttons
        yield return new WaitForSeconds(postGameOverDelay);
        if (buttonPanel)
            buttonPanel.SetActive(true);
    }

    // ------------------------
    // Respawn Countdown
    // ------------------------
    public IEnumerator ShowRespawnCountdown(int countdownTime)
    {
        StopAllCoroutines();
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);
        if (buttonPanel)
            buttonPanel.SetActive(false);

        yield return StartCoroutine(FadePanel(0.5f));

        for (int i = countdownTime; i > 0; i--)
        {
            mainText.text = $"Respawning in {i}...";

            if (audioSource && countdownBeep)
                audioSource.PlayOneShot(countdownBeep);

            yield return StartCoroutine(FadeText(1f));
            yield return new WaitForSeconds(countdownDelay);
            yield return StartCoroutine(FadeText(0f));
        }

        // Fade everything out
        StartCoroutine(FadeOutHeartbeat());
        yield return StartCoroutine(FadeOut());
    }

    // ------------------------
    // Button Events
    // ------------------------
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game (won’t exit in Editor).");
    }

    // ------------------------
    // Heartbeat Control
    // ------------------------
    private IEnumerator FadeInHeartbeat()
    {
        if (heartbeatSource == null) yield break;
        heartbeatSource.volume = 0f;
        if (!heartbeatSource.isPlaying)
            heartbeatSource.Play();

        float timer = 0f;
        while (timer < heartbeatFadeInTime)
        {
            timer += Time.unscaledDeltaTime;
            heartbeatSource.volume = Mathf.Lerp(0f, heartbeatMaxVolume, timer / heartbeatFadeInTime);
            yield return null;
        }
    }

    private IEnumerator FadeOutHeartbeat()
    {
        if (heartbeatSource == null) yield break;

        float startVol = heartbeatSource.volume;
        float timer = 0f;

        while (timer < heartbeatFadeOutTime)
        {
            timer += Time.unscaledDeltaTime;
            heartbeatSource.volume = Mathf.Lerp(startVol, 0f, timer / heartbeatFadeOutTime);
            yield return null;
        }

        heartbeatSource.Stop();
    }

    // ------------------------
    // Visual Coroutines
    // ------------------------
    private IEnumerator PulseText()
    {
        Color baseColor = mainText.color;
        while (true)
        {
            float alpha = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) / 2f;
            mainText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeText(float targetAlpha)
    {
        float timer = 0f;
        Color c = mainText.color;
        float startAlpha = c.a;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            mainText.color = c;
            yield return null;
        }
    }

    private IEnumerator FadePanel(float targetAlpha)
    {
        float timer = 0f;
        Color c = fadePanel.color;
        float startAlpha = c.a;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        yield return StartCoroutine(FadeText(0f));
        yield return StartCoroutine(FadePanel(0f));
    }

    // ------------------------
    // Helper
    // ------------------------
    private void SetAlpha(Graphic g, float alpha)
    {
        Color c = g.color;
        c.a = alpha;
        g.color = c;
    }
}
