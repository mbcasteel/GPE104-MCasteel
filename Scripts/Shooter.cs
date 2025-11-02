using System.Collections; // Add this using directive at the top of the file
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioSource gunAudio;        //  Assign in Inspector
    public AudioClip gunFireClip;       //  Sound for each shot

    [Header("Settings")]
    public float fireRate = 0.25f;

    private bool isFiring = false;
    private float nextFireTime = 0f;

    private void Update()
    {
        // Handle continuous fire
        if (isFiring && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
            isFiring = true;
        else if (context.canceled)
            isFiring = false;
    }

    private void Fire()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        //  Play gunfire sound
        SoundManager.instance.PlaySFX(SoundManager.instance.shootSound);

    }

    // In Shooter.cs
    public void ActivateRapidFire(float duration)
    {
        StartCoroutine(RapidFireRoutine(duration));
    }

    private IEnumerator RapidFireRoutine(float t)
    {
        float originalRate = fireRate;
        fireRate *= 0.5f; // double fire rate
        yield return new WaitForSeconds(t);
        fireRate = originalRate;
    }

}
