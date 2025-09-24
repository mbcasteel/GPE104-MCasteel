using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public float DamageAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Try to get a Health component from the collided Pawn
        if (collision.TryGetComponent<Pawn>(out Pawn otherPawn))
        {
            var health = otherPawn.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage((int)DamageAmount, false);
            }
        }
    }
}