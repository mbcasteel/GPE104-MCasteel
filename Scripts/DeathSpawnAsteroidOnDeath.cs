using UnityEngine;

public class DeathSpawnAsteroidOnDeath : Death
{
    public override void Die()
    {
        // Spawn a meteor
        GameManager.instance.SpawnMeteor();

        // And Destroy Self
        Destroy(gameObject);
    }
}