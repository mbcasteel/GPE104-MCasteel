using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;   // assign in Inspector
    public Transform firePoint;       // assign in Inspector
    public float bulletSpeed = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Instantiate bullet at firePoint (rotation will be fixed below)
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Determine direction:
        // Prefer firePoint.right (local +X). If your sprite faces up, consider using firePoint.up instead.
        Vector2 dir = (Vector2)firePoint.up;

        // If your player flips by changing localScale.x, account for it:
        float playerFlip = transform.localScale.x < 0f ? -1f : 1f;
        dir = new Vector2(dir.x * playerFlip, dir.y).normalized;

        // Safety fallback: if dir is nearly zero, use up
        if (dir.sqrMagnitude < 0.0001f)
            dir = (Vector2)firePoint.up;

        // Set velocity
        rb.linearVelocity = dir * bulletSpeed;

        // Rotate the bullet to face direction (so sprite points along velocity)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // debug ray to visualize in Scene view
        Debug.DrawRay(firePoint.position, dir * 1.5f, Color.red, 1f);
    }
}
