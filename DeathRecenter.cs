using UnityEngine;

public class DeathRecenter : MonoBehaviour 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {         // Recenter the game object this script is attached to
               transform.position = Vector3.zero;
    }
        
}
