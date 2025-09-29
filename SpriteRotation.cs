using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    private Transform _tf;    // Reference to our transform component
    public float _turnSpeed;  // Degrees we rotate in one frame

    void Start()
    {
        // Load our transform component into our variable
        _tf = GetComponent<Transform>();
    }

    void Update()
    {
        // Rotate "_turnSpeed" degrees on the Y axis each frame
        _tf.Rotate(0, _turnSpeed * Time.deltaTime, 0);
    }
}


