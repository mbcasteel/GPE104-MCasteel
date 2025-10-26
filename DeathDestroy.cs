using UnityEngine;

public class DeathDestroy : Death
{
    public override void Die()
    {
        // Destroy the game object that this component is on
        Destroy(this.gameObject);
    }
}