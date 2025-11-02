// PlayerShield.cs
using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    private bool active;
    public GameObject shieldVisual;

    public void ActivateShield(float duration)
    {
        if (active) return;
        active = true;
        if (shieldVisual) shieldVisual.SetActive(true);
        StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float t)
    {
        yield return new WaitForSeconds(t);
        if (shieldVisual) shieldVisual.SetActive(false);
        active = false;
    }
}
