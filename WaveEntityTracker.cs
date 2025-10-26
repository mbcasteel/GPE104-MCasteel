using UnityEngine;

public class WaveEntityTracker : MonoBehaviour
{
    public WaveSpawnerHybrid spawner;

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.OnEnemyDestroyed();
    }
}
