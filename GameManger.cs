using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int enemiesAlive = 0;

    [Header("UI")]
    public TextMeshProUGUI endText;   // Assign in Inspector
    public TextMeshProUGUI livesText; // Assign in Inspector

    [Header("Player Settings")]
    public int playerLives = 3;
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateLivesUI();
    }

    void Update()
    {
        // Allow restart if game ended
        if (gameEnded && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // ------------------------
    // Enemy Tracking
    // ------------------------
    public void RegisterEnemy()
    {
        enemiesAlive++;
    }

    public void UnregisterEnemy()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && !gameEnded)
        {
            WinGame();
        }
    }

    // ------------------------
    // Player Handling
    // ------------------------
    public void PlayerDied()
    {
        playerLives--;
        UpdateLivesUI();

        if (playerLives > 0)
        {
            Debug.Log("Player lost a life. Respawning...");
            RespawnPlayer();
        }
        else
        {
            GameOver();
        }
    }

    private void RespawnPlayer()
    {
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            GameObject newPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);

            // Give temporary invincibility
            Health health = newPlayer.GetComponent<Health>();
            if (health != null)
            {
                health.ActivateInvincibility();
            }
        }
        else
        {
            Debug.LogWarning("Player Prefab or Spawn Point not set in GameManager!");
        }
    }

    // ------------------------
    // Game End States
    // ------------------------
    public void WinGame()
    {
        EndGame("YOU WIN!\nPress R to Restart");
    }

    public void GameOver()
    {
        EndGame("GAME OVER\nPress R to Restart");
    }

    private void EndGame(string message)
    {
        Debug.Log(message.Replace("\n", " "));
        gameEnded = true;

        if (endText != null)
        {
            endText.gameObject.SetActive(true);
            endText.text = message;
        }
    }

    // ------------------------
    // Restart
    // ------------------------
    private void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        gameEnded = false;
        playerLives = 3; // reset lives

        if (endText != null)
        {
            endText.gameObject.SetActive(false);
        }

        UpdateLivesUI();
    }

    // ------------------------
    // UI Updates
    // ------------------------
    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + playerLives;
        }
    }
}
