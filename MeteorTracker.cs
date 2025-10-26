using UnityEngine;


public class MeteorTracker : MonoBehaviour
{
    public MeteorSpawnerAdvanced spawner; // Add this field

    // Add this field to allow MeteorWaveSpawner to set itself
    public MeteorSpawnerAdvanced meteorWaveSpawner; // Fixed type and field declaration

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.MeteorDestroyed();
        }

        // Notify the spawner when this meteor is destroyed
        // FIX: Remove call to non-existent OnMeteorDestroyed method
        // if (meteorWaveSpawner != null)
        // {
        //     meteorWaveSpawner.OnMeteorDestroyed();
        // }
    }

    // If you intended to have an OnMeteorDestroyed method, add it here:
    public void OnMeteorDestroyed()
    {
        // Implement logic as needed, or remove the call in MeteorTracker if not required.
    }
}
