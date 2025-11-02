using UnityEngine;

public class TriggerDeathOnEnter : MonoBehaviour
{
    public bool isTryDieOnEnter;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isTryDieOnEnter)
        {
            // Get the death component
            Death otherObjectDeathComponent = other.GetComponent<Death>();
            if (otherObjectDeathComponent != null)
            {
                otherObjectDeathComponent.Die();
            }
            else
            {
                Destroy(other.gameObject);
            }

        }
        else
        {
            //Otherwise, destroy anything that leaves the trigger
            Destroy(other.gameObject);
        }
    }
}