using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawnerHybrid : MonoBehaviour
{
    [System.Serializable]
    public class MeteorType
    {
        public string name = "Large Meteor";
        public GameObject prefab;
        [Range(0, 100)] public int spawnChance = 40;
        public float minSpeed = 0.8f;
        public float maxSpeed = 2.5f;
        public int baseHealth = 100;
        public float scale = 1.0f;
        public int fragmentsOnDeath = 2;
        public GameObject fragmentPrefab;
    }

    [Header("Meteor Types")]
    public MeteorType[] meteorTypes;

    [Header("Seeker Settings")]
    public GameObject seekerPrefab;
    [Range(0f, 1f)] public float seekerChance = 0.15f; // 15% chance per spawn after wave 2
    public float seekerSpeedMultiplier = 1.1f;
    public float seekerSpawnDistance = 16f;

    [Header("Wave Settings")]
    public int startingMeteorCount = 8;
    public int meteorIncrementPerWave = 3;
    public float spawnDelay = 0.5f;
    public float waveDelay = 3f;
    public int currentWave = 0;

    [Header("Spawn Area")]
    public float meteorSpawnDistance = 14f;
    public Transform player;

    [Header("UI")]
    public Text waveText;
    public Animator waveTextAnimator;

    [Header("Power Up Settings")]
    public GameObject[] powerUpPrefabs;
    [Range(0f, 1f)] public float dropChance = 0.3f;
    public int minDrops = 1;
    public int maxDrops = 2;
    public float dropRadius = 4f;

    private int aliveEnemies = 0;
    private bool spawningWave = false;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (!spawningWave && aliveEnemies == 0)
        {
            StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator StartNextWave()
    {
        spawningWave = true;
        currentWave++;

        // Show UI
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave}";
            waveText.gameObject.SetActive(true);
            if (waveTextAnimator != null)
                waveTextAnimator.SetTrigger("Show");
        }

        yield return new WaitForSeconds(waveDelay);

        int meteorsToSpawn = startingMeteorCount + (meteorIncrementPerWave * (currentWave - 1));

        for (int i = 0; i < meteorsToSpawn; i++)
        {
            TrySpawnMeteorOrSeeker();
            yield return new WaitForSeconds(spawnDelay);
        }

        spawningWave = false;
    }

    private void TrySpawnMeteorOrSeeker()
    {
        // After Wave 2, randomly replace some spawns with seekers
        bool spawnSeeker = seekerPrefab != null && currentWave >= 2 && Random.value < seekerChance;

        if (spawnSeeker)
        {
            SpawnSeeker();
        }
        else
        {
            SpawnMeteor();
        }
    }

    private void SpawnMeteor()
    {
        MeteorType type = GetRandomMeteorType();
        if (type == null || type.prefab == null) return;

        // Random position around player
        float angle = Random.Range(0f, 360f);
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 center = player != null ? player.position : Vector3.zero;
        Vector3 spawnPos = center + (Vector3)(spawnDir * meteorSpawnDistance);

        GameObject meteorGO = Instantiate(type.prefab, spawnPos, Quaternion.identity);
        aliveEnemies++;

        meteorGO.transform.localScale = Vector3.one * type.scale;
        Meteor meteor = meteorGO.GetComponent<Meteor>();
        if (meteor != null)
        {
            meteor.maxHealth = type.baseHealth;
            meteor.fragmentsToSpawn = type.fragmentsOnDeath;
            meteor.fragmentPrefab = type.fragmentPrefab;
        }

        Rigidbody2D rb = meteorGO.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 driftDir = (center - spawnPos).normalized;
            float speed = Random.Range(type.minSpeed, type.maxSpeed);
            rb.linearVelocity = driftDir * speed;
            rb.angularVelocity = Random.Range(-60f, 60f);
        }

        meteorGO.AddComponent<WaveEntityTracker>().spawner = this;
    }

    private void SpawnSeeker()
    {
        if (seekerPrefab == null || player == null) return;

        float angle = Random.Range(0f, 360f);
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 spawnPos = player.position + (Vector3)(spawnDir * seekerSpawnDistance);

        GameObject seeker = Instantiate(seekerPrefab, spawnPos, Quaternion.identity);
        aliveEnemies++;

        // Tune speed scaling per wave
        SeekerMover mover = seeker.GetComponent<SeekerMover>();
        if (mover != null)
            mover.moveSpeed *= (1f + (currentWave - 1) * seekerSpeedMultiplier * 0.05f);

        seeker.AddComponent<WaveEntityTracker>().spawner = this;
    }

    private MeteorType GetRandomMeteorType()
    {
        int total = 0;
        foreach (var t in meteorTypes) total += t.spawnChance;
        int roll = Random.Range(0, total);
        int cumulative = 0;
        foreach (var t in meteorTypes)
        {
            cumulative += t.spawnChance;
            if (roll < cumulative)
                return t;
        }
        return meteorTypes[0];
    }

    public void OnEnemyDestroyed()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);

        // If all enemies are gone and not currently spawning a new wave
        if (aliveEnemies == 0 && !spawningWave)
        {
            // Chance to spawn rewards before next wave
            StartCoroutine(SpawnPowerUpsAfterClear());
        }
    }

    private IEnumerator SpawnPowerUpsAfterClear()
    {
        yield return new WaitForSeconds(1f); // slight pause after last enemy

        if (powerUpPrefabs.Length == 0 || Random.value > dropChance)
            yield break;

        int dropCount = Random.Range(minDrops, maxDrops + 1);

        for (int i = 0; i < dropCount; i++)
        {
            GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

            // Random point near center or player
            Vector3 center = player != null ? player.position : Vector3.zero;
            Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(1f, dropRadius);
            Vector3 dropPos = center + (Vector3)offset;

            Instantiate(prefab, dropPos, Quaternion.identity);
        }
    }
}
