using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public Image livesBar;
    public Gradient barGradient;

    void Start()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnScoreChanged += UpdateScoreUI;
        // Initialize display
        UpdateScoreUI(GameManager.instance != null ? GameManager.instance.playerScore : 0);
    }

    public void UpdateScoreUI(int newScore)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {newScore}";
    }

    private void Update()
    {
        // Ensure GameManager and player references exist
        if (GameManager.instance == null) return;

        // ? Update score
        scoreText.text = $"Score: {GameManager.instance.playerScore}";

        // ? Update lives (if playerController exists)
        if (GameManager.instance.playerController != null &&
            GameManager.instance.playerController.pawn != null)
        {
            int currentLives = GameManager.instance.playerController.pawn.lives;
            int maxLives = GameManager.instance.playerLives;

            float ratio = maxLives > 0 ? (float)currentLives / maxLives : 0f;
            livesText.text = $"Lives: {currentLives}";
            livesBar.fillAmount = ratio;
            livesBar.color = barGradient.Evaluate(ratio);
        }
    }
}
