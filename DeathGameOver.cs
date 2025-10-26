using UnityEngine;

public class DeathGameOver : Death
{
    private ControllerPlayer controller;

    void Start()
    {
        controller = Object.FindFirstObjectByType<ControllerPlayer>();
    }

    public override void Die()
    {
        if (controller != null)
        {
            GameManager.instance.HandlePlayerDeath(controller);
        }

        Destroy(gameObject);
    }
}