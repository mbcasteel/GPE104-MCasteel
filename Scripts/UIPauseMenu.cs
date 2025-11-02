using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject pausePanel;     // the UI panel for pause menu
    public UISettings settingsPanel;  // reference to your settings script
    public GameObject settingsObject; // the actual settings UI GameObject

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause when pressing Escape or Start
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);
        if (settingsObject != null)
            settingsObject.SetActive(false);

        // fade music down a bit
        if (SoundManager.instance != null)
            SoundManager.instance.FadeMusicTo(0.15f, 1f);

        // play UI click sound
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pausePanel.SetActive(false);

        // fade music back up
        if (SoundManager.instance != null)
            SoundManager.instance.FadeMusicTo(SoundManager.instance.musicVolume, 1f);

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
    }

    public void OpenSettings()
    {
        if (settingsObject != null)
            settingsObject.SetActive(true);

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
    }

    public void CloseSettings()
    {
        if (settingsObject != null)
            settingsObject.SetActive(false);

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);

        // Example: load main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
