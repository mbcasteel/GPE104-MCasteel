using UnityEngine;

public class DeathDestroy : MonoBehaviour

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
    {         // Destroy the game object this script is attached to
        Destroy(this.gameObject);
    }
}
