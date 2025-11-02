using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // Add this field to track the number of enemies
    public int currentNumberOfEnemies;
    public event System.Action<int> OnScoreChanged;

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Transform spawnPoint;
    public ControllerPlayer playerController;

    [Header("Game State")]
    public int playerLives = 3;
    public int playerScore = 0;
    public int scoreToWin = 10000; //  Set your win target here
    private bool gameEnded = false;

    [Header("UI References")]
    public GameOverUI gameOverUI;
    public UIGameplay gameplayUI;
    public UIMainMenu mainMenuUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SpawnPlayer();
    }

    // 🧍 PLAYER
    public void SpawnPlayer()
    {
        if (gameEnded) return;

        if (playerPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Missing playerPrefab or spawnPoint!");
            return;
        }

        GameObject newPawn = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PawnSpaceship pawn = newPawn.GetComponent<PawnSpaceship>();

        if (playerController == null)
            playerController = FindFirstObjectByType<ControllerPlayer>();

        if (playerController != null)
            playerController.pawn = pawn;

        pawn?.ActivateInvincibility();
        Debug.Log("Player spawned with invincibility");
    }

    public void HandlePlayerDeath(ControllerPlayer controller)
    {
        // Reduce player lives
        playerLives--;
        Debug.Log($"Player lost a life. Lives remaining: {playerLives}");

        if (playerLives > 0)
        {
            // Start respawn countdown
            StartCoroutine(RespawnRoutine());
        }
        else
        {
            // No lives left — Game Over
            GameOver();
        }
    }

    private IEnumerator RespawnRoutine()
    {
        // Optional UI countdown (reuse gameOverUI countdown if you like)
        if (gameOverUI != null)
            yield return StartCoroutine(gameOverUI.ShowRespawnCountdown(3));

        yield return new WaitForSeconds(0.25f);

        // Spawn new player pawn
        SpawnPlayer();
    }

    // SCORING
    public void AddScore(int amount)
    {
        if (gameEnded) return;

        playerScore += amount;
        gameplayUI?.UpdateScoreUI(playerScore);

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.instance.hitSound);

        Debug.Log($"Score: {playerScore}");

        if (playerScore >= scoreToWin)
            WinGame();
    }

    // GAME OVER
    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("GAME OVER");

        if (SoundManager.instance != null)
            SoundManager.instance.PlayMusic(SoundManager.instance.gameOverMusic);

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }

    // WIN CONDITION
    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("YOU WIN!");

        if (SoundManager.instance != null)
            SoundManager.instance.PlayMusic(SoundManager.instance.gameplayMusic); // or a victory clip

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }

    //  QUIT
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Add this method to the GameManager class
    public void SpawnMeteor()
    {
        // TODO: Implement meteor spawning logic here
        // Example: Instantiate(meteorPrefab, somePosition, Quaternion.identity);
    }
}
