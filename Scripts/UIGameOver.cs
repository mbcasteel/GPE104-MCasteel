using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [Header("UI References")]
    public Image fadePanel;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subText;
    public Button retryButton;
    public Button quitButton;

    [Header("Settings")]
    public float fadeDuration = 1.5f;

    private bool isGameOverShown = false;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (fadePanel != null)
            fadePanel.color = new Color(0, 0, 0, 0);

        if (mainText != null)
            mainText.color = new Color(1, 1, 1, 0);

        if (subText != null)
            subText.color = new Color(1, 1, 1, 0);
    }

    // Called by GameManager when lives = 0
    public void ShowGameOver(string message = "GAME OVER")
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        mainText.text = message;
        subText.text = "";
        StartCoroutine(FadeInRoutine());
        isGameOverShown = true;
    }

    private IEnumerator FadeInRoutine()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            if (fadePanel != null)
                fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0, 0.75f, t));

            if (mainText != null)
                mainText.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));

            if (subText != null)
                subText.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));

            yield return null;
        }

        if (subText != null)
            subText.text = "Press [R] to Retry or [Esc] to Quit";

        if (retryButton != null)
            retryButton.gameObject.SetActive(true);

        if (quitButton != null)
            quitButton.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isGameOverShown) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.instance.LoadScene("Gameplay");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.instance.LoadScene("MainMenu");
        }
    }

    // For respawn countdown between lives
    public IEnumerator ShowRespawnCountdown(int seconds)
    {
        gameObject.SetActive(true);

        if (fadePanel != null)
            fadePanel.color = new Color(0, 0, 0, 0.5f);

        if (mainText != null)
            mainText.text = "";

        if (subText != null)
        {
            for (int i = seconds; i > 0; i--)
            {
                subText.text = $"Respawning in {i}...";
                yield return new WaitForSeconds(1f);
            }

            subText.text = "";
        }
        else
        {
            for (int i = seconds; i > 0; i--)
                yield return new WaitForSeconds(1f);
        }

        gameObject.SetActive(false);
    }
}
