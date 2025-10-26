using UnityEngine;

public class DirectionMover : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public Vector3 moveDirection;


    private void Start()
    {
        // Choose a random direction to move
        Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        moveDirection = randomDirection;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = moveDirection.normalized;
        moveVector = moveVector * moveSpeed;
        moveVector = moveVector * Time.deltaTime;

        // Actually move!
        transform.position = transform.position + moveVector;


    }
}