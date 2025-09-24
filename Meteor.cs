using UnityEngine;

public class Meteor : MonoBehaviour
{
    public bool isInstantDeath = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with " + other.name); // Debugging

        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(1, isInstantDeath);
            }
        }
    }
}
