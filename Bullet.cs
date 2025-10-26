using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public int damage = 50;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Meteor meteor = other.GetComponent<Meteor>();
        if (meteor != null)
        {
            meteor.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
