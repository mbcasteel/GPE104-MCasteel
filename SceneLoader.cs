using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load a scene by name
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Optional fade-out music
        if (SoundManager.instance != null)
            SoundManager.instance.FadeMusicTo(0f, 1f);

        yield return new WaitForSecondsRealtime(1f); // give fade a second

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // Play appropriate music for the new scene
        if (sceneName == "MainMenu" && SoundManager.instance.menuMusic != null)
            SoundManager.instance.PlayMusic(SoundManager.instance.menuMusic, true);
        else if (sceneName == "Gameplay" && SoundManager.instance.gameplayMusic != null)
            SoundManager.instance.PlayMusic(SoundManager.instance.gameplayMusic, true);
        else if (sceneName == "GameOver" && SoundManager.instance.gameOverMusic != null)
            SoundManager.instance.PlayMusic(SoundManager.instance.gameOverMusic, true);
    }

    // Quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();                         
#endif
    }
}
