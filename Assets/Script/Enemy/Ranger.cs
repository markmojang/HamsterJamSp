using UnityEngine;

public class Ranger : Enemy
{
    public float rotationSpeed = 5f;
    public float minDistance = 3f;  // Minimum distance from player
    public float maxDistance = 6f;  // Maximum distance from player
    public float attackRange = 10f;
    public GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullets will be shot
    public float bulletSpeed = 15f; // Speed of the bullets
    public float fireRate = 0.2f; // Time between shots in seconds

    private float nextFireTime = 0f;

    protected override void Start()
    {
        health = 100f;
        damage = 20f;
        speed = 2f;

        base.Start();
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void Update()
    {
        MaintainDistanceFromPlayer();
        LookAtPlayer();
        
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void MaintainDistanceFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < minDistance)
        {
            // Move away from the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        else if (distance > maxDistance)
        {
            // Move closer to the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void LookAtPlayer()
    {
        Vector2 direction = new Vector2(
            player.position.x - transform.position.x,
            player.position.y - transform.position.y
        );

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        float currentAngle = transform.rotation.eulerAngles.z;
        
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        EnemyBullet DMG = bullet.GetComponent<EnemyBullet>();
        DMG.damage = damage;

        if (rb != null)
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
    }
}
