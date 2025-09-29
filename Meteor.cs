using UnityEngine;

public class Meteor : MonoBehaviour
{
    public bool isInstantDeath = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                // Deal 1 damage (or instant death if flag is true)
                health.TakeDamage(1, isInstantDeath);

                // Meteor stays — do NOT destroy it here
            }
        }
    }
}
