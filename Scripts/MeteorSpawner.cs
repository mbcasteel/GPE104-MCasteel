using System.Collections;
using UnityEngine;

public class MeteorSpawnerAdvanced : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject meteorPrefab;
    public int startCount = 5;         // Meteors in first wave
    public float spawnRadius = 12f;
    public float waveDelay = 3f;
    public int waveIncrease = 3;       // Add more meteors per wave
    public float driftMultiplier = 1.2f; // Increases meteor drift speed each wave
    private UIWaveAnnouncement waveUI;

    private int currentWave = 0;
    private int meteorsRemaining = 0;

    private void Start()
    {
        waveUI = FindFirstObjectByType<UIWaveAnnouncement>();
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(waveDelay);
        currentWave++;

        int meteorCount = startCount + (currentWave - 1) * waveIncrease;
        Debug.Log($"🪐 Spawning Wave {currentWave} ({meteorCount} meteors)");

        for (int i = 0; i < meteorCount; i++)
        {
            SpawnRandomMeteor();
        }
    }

    public void SpawnRandomMeteor()
    {
        if (meteorPrefab == null)
        {
            Debug.LogWarning("⚠️ Meteor prefab not assigned!");
            return;
        }

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 spawnPos = randomDir * spawnRadius;
        GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        meteorsRemaining++;

        // Adjust difficulty
        Meteor meteorScript = meteor.GetComponent<Meteor>();
        if (meteorScript != null)
        {
            meteorScript.driftSpeedMin *= Mathf.Pow(driftMultiplier, currentWave - 1);
            meteorScript.driftSpeedMax *= Mathf.Pow(driftMultiplier, currentWave - 1);

            Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (Vector2.zero - (Vector2)spawnPos).normalized;
                rb.linearVelocity = dir * Random.Range(meteorScript.driftSpeedMin, meteorScript.driftSpeedMax);
                rb.angularVelocity = Random.Range(meteorScript.spinSpeedMin, meteorScript.spinSpeedMax);
            }
        }
    }

    // Called by Meteor.cs → OnDestroy()
    public void MeteorDestroyed()
    {
        meteorsRemaining = Mathf.Max(0, meteorsRemaining - 1);

        if (meteorsRemaining <= 0)
        {
            Debug.Log($"🌌 Wave {currentWave} cleared!");

            // Award bonus points for wave completion
            if (GameManager.instance != null)
                GameManager.instance.AddScore(250);

            // Trigger next wave or win condition
            if (currentWave >= 5) // 🏆 Win after 5 waves (you can tweak this)
                GameManager.instance.WinGame();
            else
                StartCoroutine(StartNextWave());
        }
    }
}
