using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;        // assign ScreenFader CanvasGroup
    public float fadeDuration = 1f;

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

    private void Start()
    {
        // Fade in when the scene first loads
        if (fadeCanvas != null)
            StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Fade out to black
        if (fadeCanvas != null)
            yield return StartCoroutine(FadeOut());

        // Optionally fade down music
        if (SoundManager.instance != null)
            SoundManager.instance.FadeMusicTo(0f, fadeDuration);

        // Wait a bit
        yield return new WaitForSecondsRealtime(0.3f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // Play proper music after scene load
        if (SoundManager.instance != null)
        {
            if (sceneName == "MainMenu")
                SoundManager.instance.PlayMusic(SoundManager.instance.menuMusic, true);
            else if (sceneName == "Gameplay")
                SoundManager.instance.PlayMusic(SoundManager.instance.gameplayMusic, true);
        }

        // Fade from black to clear
        if (fadeCanvas != null)
            yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        fadeCanvas.blocksRaycasts = false;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = 0f;
    }

    private IEnumerator FadeOut()
    {
        fadeCanvas.blocksRaycasts = true;
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = 1f;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
