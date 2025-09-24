using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // This matches what your Meteor script calls:
    public void TakeDamage(int damage, bool isInstantDeath)
    {
        if (isInstantDeath)
        {
            currentHealth = 0;
            Debug.Log(gameObject.name + " was instantly destroyed!");
        }
        else
        {
            currentHealth -= damage;
            Debug.Log(gameObject.name + " took " + damage + " damage. Health = " + currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        // You can add destroy logic, respawn, animation, etc.
        Destroy(gameObject);
    }
}
