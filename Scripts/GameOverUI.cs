using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Image fadePanel;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subText;
    public Button retryButton;
    public Button quitButton;
    public float fadeDuration;

    private void Awake()
    {
        // Existing initialization code
    }

    public void ShowGameOver(string message = "GAME OVER")
    {
        // Existing implementation
    }

    private IEnumerator FadeInRoutine()
    {
        // Existing implementation
        yield break;
    }

    private void Update()
    {
        // Existing implementation
    }

    // Add this method to support respawn countdown
    public IEnumerator ShowRespawnCountdown(int seconds)
    {
        if (mainText != null)
            mainText.text = "Respawning...";

        if (subText != null)
        {
            for (int i = seconds; i > 0; i--)
            {
                subText.text = $"Respawn in {i}...";
                yield return new WaitForSeconds(1f);
            }
            subText.text = "";
        }
        else
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}