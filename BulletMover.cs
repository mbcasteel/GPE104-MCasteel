using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public float moveSpeed;
    public DamageOnCollision damageOnHitComponent;

    public void Awake()
    {
        // load our component variables
        damageOnHitComponent = GetComponent<DamageOnCollision>();
    }

    void Update()
    {
        //move the bullet forward
        transform.position = transform.position + (transform.up * moveSpeed * Time.deltaTime); 
    }
}
