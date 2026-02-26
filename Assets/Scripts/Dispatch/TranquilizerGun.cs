using UnityEngine;

public class TranquilizerGun : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab; 
    public Transform firePoint;          // Empty GameObject at gun tip
    public float projectileSpeed = 20f;  // How fast the projectile is
    public float projectileLife = 5f;    // How long the projectile exists

    [Header("Gun Settings")]
    public float fireRate = 0.5f;        // Seconds between shots
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        // Spawn the projectile at FirePoint
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        // Destroy after lifetime
        Destroy(proj, projectileLife);
    }
}