using UnityEngine;

public class BulletMover : MonoBehaviour
{
    public float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move forward
        transform.position = transform.position + (transform.up * moveSpeed * Time.deltaTime);
    }
}