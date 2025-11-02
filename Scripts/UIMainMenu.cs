using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    private void Start()
    {
        if (SoundManager.instance != null && SoundManager.instance.menuMusic != null)
            SoundManager.instance.PlayMusic(SoundManager.instance.menuMusic, true);

        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnPlayButton()
    {
        Debug.Log("?? Play clicked");
        SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
        SceneLoader.instance.LoadScene("Gameplay");
    }

    public void OnSettingsButton()
    {
        SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnBackButton()
    {
        SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnQuitButton()
    {
        SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
        SceneLoader.instance.QuitGame();
    }
}
