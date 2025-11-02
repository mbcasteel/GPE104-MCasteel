using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 3f;
    public int damage = 50;

    private void Start()
    {
        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move forward relative to its facing direction
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Damage Seeker enemies
        SeekerHealth seeker = other.GetComponent<SeekerHealth>();
        if (seeker != null)
        {
            seeker.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Damage Meteors
        Meteor meteor = other.GetComponent<Meteor>();
        if (meteor != null)
        {
            meteor.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Optionally destroy bullet on environment or other enemies
        if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
