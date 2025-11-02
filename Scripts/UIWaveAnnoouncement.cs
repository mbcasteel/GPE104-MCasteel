using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWaveAnnouncement : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI waveText;
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    public float fadeDuration = 1f;
    public float stayDuration = 2.5f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void ShowWaveMessage(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeMessageRoutine(message));
    }

    private IEnumerator FadeMessageRoutine(string message)
    {
        waveText.text = message;

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(stayDuration);

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
