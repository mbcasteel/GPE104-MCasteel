using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add one to my current number of enemies
        GameManager.instance.currentNumberOfEnemies++;
    }

    void OnDestroy()
    {
        // Subtract one from my current number of enemies
        GameManager.instance.currentNumberOfEnemies--;
    }
}