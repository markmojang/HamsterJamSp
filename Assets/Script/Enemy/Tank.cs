using UnityEngine;

public class Tank : Enemy
{
    public float rotationSpeed = 5f;
    public float minDistance = 1f; 
    public float attackRange = 2f;
    public GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullets will be shot
    public float bulletSpeed = 10f; // Speed of the bullets
    public float fireRate = 3f; // Time between shots in seconds
    public int bulletCount = 5; // Number of bullets in the shotgun spread
    public float spreadAngle = 30f; // Total spread angle in degrees

    private float nextFireTime = 0f;

    protected override void Start()
    {
        health = 200f;
        damage = 10f;
        speed = 1.0f;

        base.Start();
    }

    private void Update()
    {
        MoveTowardsPlayer();
        LookAtPlayer();
        
        if (Time.time >= nextFireTime)
        {
            FireShotgun();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < minDistance)
        {
            // Move away from the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        else
        {
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

    private void FireShotgun()
    {
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, 0, currentAngle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            EnemyBullet DMG = bullet.GetComponent<EnemyBullet>();
            DMG.damage = damage;

            if (rb != null)
            {
                Vector3 direction = bulletRotation * Vector3.up;
                rb.velocity = direction * bulletSpeed;
            }
        }
    }
}
