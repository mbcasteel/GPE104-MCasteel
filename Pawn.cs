using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Pawn : MonoBehaviour
{
    // Variable to hold the Transform component
    private Transform tf;
    // Variable to hold editable local movement (WASD) Unit per second
    public float moveSpeed = 5f;
    // Variable to hold editable World movement (Arrow Keys) Units per second
    public float worldMoveSpeed = 5f;
    // Variable to hold editable turbo speed
    public float turboSpeed = 10f;
    // Variable to hold degree rotation per frame
    public float rotateSpeed = 200f;

    public float minX = -10f;

    public float maxX = 10f;

    public float minY = -5f;

    public float maxY = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Transform component
        tf = GetComponent<Transform>();
        // Start at the world origin point (0,0,0)

        tf.position = Vector3.zero;

    }
    // Local Movement (relative)
    // Forward movement (Local) relative to the direction the object is facing
    public void MoveForward()
    {
        tf.position += (tf.up * moveSpeed * Time.deltaTime);  
    }
    public void MoveBackward()
    {
        tf.position -= (tf.up * moveSpeed * Time.deltaTime);
    }
    // Rotate Left (Local) along Z axis



    public void RotateLeft()
    {
        tf.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
    // Rotate Right (Local)
    public void RotateRight()
    {
        tf.Rotate(tf.forward, -rotateSpeed * Time.deltaTime);
        }
    // World Movement (absolute)
    // Slide forward
    public void MoveUpWorld()
    {
        tf.position = tf.position + (Vector3.up * worldMoveSpeed * Time.deltaTime);
    }
    // Slide backward
    public void MoveDownWorld()
    {
        tf.position = tf.position + (Vector3.down * worldMoveSpeed * Time.deltaTime);
    }
    // Strafe left
    public void MoveLeftWorld()
    {
        tf.position = tf.position + (Vector3.left * worldMoveSpeed * Time.deltaTime);
    }
    // Strafe right
    public void MoveRightWorld()
    {
        tf.position = tf.position + (Vector3.right * worldMoveSpeed * Time.deltaTime);
    }

    // Teleport to a random location within bounds  
    public void TeleportRandom()
    {
        // Generate random x and y coordinates within specified bounds
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        // Set the object's position to the new random coordinates
        tf.position = new Vector3(randomX, randomY, 0);
        // Console log for debugging
        Debug.Log("Slingshot engaged!");
    }
    // Turbo forward movement (Local) relative to the direction the object is facing
    public void MoveTurboForward()
    {
        tf.position = tf.position + (tf.up * turboSpeed * Time.deltaTime);
    }
}