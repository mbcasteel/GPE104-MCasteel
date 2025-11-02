using UnityEngine;

public class DeathRecenter : Death
{
    public override void Die()
    {
        // Move the object back to the center of the universe
        transform.position = Vector3.zero;
    }
}