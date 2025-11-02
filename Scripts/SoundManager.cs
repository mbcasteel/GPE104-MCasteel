using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip gameOverMusic;

    [Header("Sound Effects")]
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioClip hitSound;
    public AudioClip buttonClickSound;

    [Header("Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.2f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;
    public float fadeDuration = 1.5f;

    private Coroutine fadeRoutine;

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
        // Start menu music by default
        if (menuMusic != null)
            PlayMusic(menuMusic, true);
    }

    // Fade to a new music track
    public void PlayMusic(AudioClip newTrack, bool fade = true)
    {
        if (musicSource == null || newTrack == null) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeMusicRoutine(newTrack, fade));
    }

    // Fade current music to a specific volume (used for pause/resume)
    public void FadeMusicTo(float targetValue, float duration = 1f)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeMusicVolumeRoutine(targetValue, duration));
    }

    private IEnumerator FadeMusicRoutine(AudioClip newTrack, bool fade)
    {
        if (fade)
        {
            // Fade out
            float startVol = musicSource.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVol, 0, t / fadeDuration);
                yield return null;
            }
            musicSource.volume = 0;
        }

        // Swap track
        musicSource.clip = newTrack;
        musicSource.loop = true;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, musicVolume, t / fadeDuration);
            yield return null;
        }
        musicSource.volume = musicVolume;
    }

    //Coroutine that fades to a specific volume
    private IEnumerator FadeMusicVolumeRoutine(float targetVolume, float duration)
    {
        float startVol = musicSource.volume;
        float timer = 0f;

        // Use unscaled time so it still fades while game is paused
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVol, targetVolume, timer / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    //Play SFX one-shot
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // Volume Control
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}
