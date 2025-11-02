using UnityEngine;
using System.Collections;

public class SeekerSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject seekerPrefab;
    public float spawnRadius = 15f;
    public float spawnInterval = 5f;
    public int maxSeekers = 10;

    private int currentSeekers = 0;

    private void Start()
    {
        StartCoroutine(SpawnSeekersLoop());
    }

    private IEnumerator SpawnSeekersLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Prevent over-spawning
            if (currentSeekers >= maxSeekers)
                continue;

            SpawnSeeker();
        }
    }

    private void SpawnSeeker()
    {
        if (seekerPrefab == null)
        {
            Debug.LogWarning("?? No seeker prefab assigned!");
            return;
        }

        // Pick random spawn point around the player or world origin
        Vector2 spawnDir = Random.insideUnitCircle.normalized;
        Vector2 spawnPos = spawnDir * spawnRadius;

        GameObject newSeeker = Instantiate(seekerPrefab, spawnPos, Quaternion.identity);
        currentSeekers++;

        // Optional: give them a random slight rotation
        newSeeker.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        // Hook into death cleanup (if seeker is destroyed)
        SeekerDeathHandler deathHandler = newSeeker.AddComponent<SeekerDeathHandler>();
        deathHandler.spawner = this;
    }

    public void NotifySeekerDestroyed()
    {
        currentSeekers = Mathf.Max(0, currentSeekers - 1);
    }
}

public class SeekerDeathHandler : MonoBehaviour
{
    [HideInInspector] public SeekerSpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.NotifySeekerDestroyed();
    }
}
