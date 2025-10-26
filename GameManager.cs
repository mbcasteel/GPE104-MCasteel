using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]
    public GameObject playerPrefab;          // Your Pawn prefab
    public Transform spawnPoint;             // Where to spawn new Pawns
    public ControllerPlayer playerController; // Reference to the player’s controller

    [Header("Game State")]
    public int playerLives = 3;
    public int playerScore = 0;

    [Header("UI References")]
    public GameOverUI gameOverUI;
    public UIGameplay gameplayUI;
    public UIMainMenu mainMenuUI;

    [Header("Enemy Tracking")]
    public int currentNumberOfEnemies;

    // Event for UI to listen for score updates
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        // Singleton setup
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SpawnPlayer();

        // Initialize UI with default score
        if (gameplayUI != null)
            gameplayUI.UpdateScoreUI(playerScore);
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null || spawnPoint == null)
        {
            Debug.LogError("❌ GameManager missing playerPrefab or spawnPoint!");
            return;
        }

        GameObject newPawn = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PawnSpaceship pawnComponent = newPawn.GetComponent<PawnSpaceship>();

        if (playerController == null)
            playerController = FindFirstObjectByType<ControllerPlayer>();

        if (playerController != null)
            playerController.pawn = pawnComponent;

        // 🛡 Activate respawn protection
        if (pawnComponent != null)
            pawnComponent.ActivateInvincibility();

        Debug.Log("✅ Player spawned (invincible for a few seconds).");

        SoundManager.instance.PlayMusic(SoundManager.instance.gameplayMusic, true);

    }

    private IEnumerator RespawnCountdownRoutine()
    {
        if (gameOverUI != null)
            yield return StartCoroutine(gameOverUI.ShowRespawnCountdown(3));

        yield return new WaitForSeconds(0.2f);

        GameObject newPawnGO = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Pawn newPawn = newPawnGO.GetComponent<Pawn>();

        if (playerController != null)
            playerController.pawn = newPawn as PawnSpaceship;
        else
        {
            playerController = FindFirstObjectByType<ControllerPlayer>();
            if (playerController != null)
                playerController.pawn = newPawn as PawnSpaceship;
        }
    }

    public void GameOver()
    {
        Debug.Log("💀 GAME OVER! All lives lost.");

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
        else
            Debug.LogWarning("⚠️ GameOverUI not assigned in GameManager.");

        SoundManager.instance.PlayMusic(SoundManager.instance.gameOverMusic, true);

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // TODO: Implement meteor spawning here
    public void SpawnMeteor()
    {
        // Example: Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
    }

    public void HandlePlayerDeath(ControllerPlayer controller)
    {
        controller.lives--;

        if (controller.lives > 0)
            StartCoroutine(RespawnRoutine());
        else
            GameOver();
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(0.25f); // ensure old pawn destroyed
        SpawnPlayer();
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
        Debug.Log($"🏆 Score: {playerScore}");

        // Notify UI
        OnScoreChanged?.Invoke(playerScore);

        // Optionally, update the UI directly if event isn’t hooked
        if (gameplayUI != null)
            gameplayUI.UpdateScoreUI(playerScore);
    }
}
