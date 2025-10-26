using UnityEngine;
using System.Collections;

public class HeartbeatController : MonoBehaviour
{
    [Header("Heartbeat Audio")]
    public AudioSource heartbeatSource;   // assign looping heartbeat clip
    public float fadeInTime = 1.5f;       // time to fade in
    public float fadeOutTime = 1.0f;      // time to fade out
    public float targetVolume = 1f;       // max volume during fade in

    private Coroutine currentRoutine;

    // Call this when the player dies
    public void StartHeartbeat()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeInHeartbeat());
    }

    // Call this when the Game Over or respawn finishes
    public void StopHeartbeat()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeOutHeartbeat());
    }

    private IEnumerator FadeInHeartbeat()
    {
        if (heartbeatSource == null) yield break;
        heartbeatSource.volume = 0f;
        heartbeatSource.loop = true;
        heartbeatSource.Play();

        float timer = 0f;
        while (timer < fadeInTime)
        {
            timer += Time.unscaledDeltaTime;
            heartbeatSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeInTime);
            yield return null;
        }
    }

    private IEnumerator FadeOutHeartbeat()
    {
        if (heartbeatSource == null) yield break;
        float startVol = heartbeatSource.volume;
        float timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.unscaledDeltaTime;
            heartbeatSource.volume = Mathf.Lerp(startVol, 0f, timer / fadeOutTime);
            yield return null;
        }

        heartbeatSource.Stop();
    }
}
