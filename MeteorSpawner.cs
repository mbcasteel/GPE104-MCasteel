using UnityEngine;

public class MeteorSpawnerAdvanced : MonoBehaviour
{
    [System.Serializable]
    public class MeteorType
    {
        public string name = "Big Meteor";
        public GameObject prefab;
        [Range(0, 100)] public int spawnChance = 40; // % chance
        public float minSpeed = 0.5f;
        public float maxSpeed = 2.5f;
        public int baseHealth = 100;
        public float scale = 1.0f;
        public int fragmentsOnDeath = 2;
        public GameObject fragmentPrefab; // optional smaller meteor
    }

    [Header("Spawn Settings")]
    public MeteorType[] meteorTypes;
    public float spawnInterval = 2f;
    public int maxMeteors = 20;

    [Header("Spawn Area")]
    public float spawnDistance = 14f;
    public Transform player; // optional — spawn around player
    public float minAngle = 0f;
    public float maxAngle = 360f;

    [Header("Power-Up Settings")]
    public GameObject[] powerUpPrefabs;     // drag prefabs here
    [Range(0f, 1f)] public float dropChance = 0.6f; // 60% chance per wave
    public int minDrops = 1;
    public int maxDrops = 2;
    public float dropRadius = 6f;


    private float nextSpawnTime;
    private int currentMeteorCount;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnMeteor();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void TrySpawnMeteor()
    {
        if (meteorTypes.Length == 0 || currentMeteorCount >= maxMeteors)
            return;

        MeteorType chosenType = GetRandomMeteorType();
        if (chosenType == null || chosenType.prefab == null)
            return;

        // Random spawn position around player or origin
        float angle = Random.Range(minAngle, maxAngle);
        Vector2 spawnDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 center = player != null ? player.position : Vector3.zero;
        Vector3 spawnPos = center + (Vector3)(spawnDir * spawnDistance);

        // Spawn meteor
        GameObject meteorGO = Instantiate(chosenType.prefab, spawnPos, Quaternion.identity);
        currentMeteorCount++;

        // Configure meteor
        meteorGO.transform.localScale = Vector3.one * chosenType.scale;
        Meteor meteor = meteorGO.GetComponent<Meteor>();
        if (meteor != null)
        {
            meteor.maxHealth = chosenType.baseHealth;
            meteor.fragmentsToSpawn = chosenType.fragmentsOnDeath;
            meteor.fragmentPrefab = chosenType.fragmentPrefab;
        }

        // Set drift toward center/player
        Rigidbody2D rb = meteorGO.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 driftDir = (center - spawnPos).normalized;
            float speed = Random.Range(chosenType.minSpeed, chosenType.maxSpeed);
            rb.linearVelocity = driftDir * speed;
            rb.angularVelocity = Random.Range(-60f, 60f);
        }

        // Register meteor destruction
        meteorGO.AddComponent<MeteorTracker>().spawner = this;
    }

    MeteorType GetRandomMeteorType()
    {
        int total = 0;
        foreach (var t in meteorTypes)
            total += t.spawnChance;

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

    public void MeteorDestroyed()
    {
        currentMeteorCount = Mathf.Max(0, currentMeteorCount - 1);
    }
}
