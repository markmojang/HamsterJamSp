using UnityEngine;

public class Sniper : Enemy
{
    public float rotationSpeed = 5f;
    public float attackRange = 10f;
    public GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullet will be shot
    public float bulletSpeed = 20f; // Speed of the bullet
    public float fireRate = 2f; // Time between shots in seconds

    private float nextFireTime = 0f;

    protected override void Start()
    {
        health = 50f;
        damage = 50f;
        speed = 1.5f;

        base.Start();
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

        if (distance < attackRange)
        {
            // Move away from the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        else if (distance > attackRange)
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
