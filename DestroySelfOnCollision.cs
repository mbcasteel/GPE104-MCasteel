using UnityEngine;

public class DestroySelfOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);
    }
}