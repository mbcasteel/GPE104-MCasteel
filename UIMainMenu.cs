using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
        SceneLoader.instance.LoadScene("Gameplay");
    }

    public void OpenSettings()
    {
        SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
        // Enable your settings panel GameObject
    }

    public void QuitGame()
    {
        SoundManager.instance.PlaySFX(SoundManager.instance.buttonClickSound);
        SceneLoader.instance.QuitGame();
    }
}
