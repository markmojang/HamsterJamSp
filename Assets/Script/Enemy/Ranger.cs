using UnityEngine;

public class Ranger : Enemy
{
    public float rotationSpeed = 5f;
    public float surroundingRadius = 20f; // Desired distance from player
    public float orbitSpeed = 45f; // Speed at which enemies orbit around the player
    public float attackRange = 10f;
    public GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullets will be shot
    public float bulletSpeed = 15f; // Speed of the bullets
    public float fireRate = 0.2f; // Time between shots in seconds

    private float nextFireTime = 0f;
    private WaveManager waveManager;
    private int currentWave;
    private float currentAngle;

    protected override void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        currentWave = waveManager.currentWave - 1;
        float multiplier = 1f + (currentWave * 0.1f);
        health = 100f * multiplier;
        damage = 30f * multiplier;
        speed = 15f;

        // Assign a random initial angle for this enemy
        currentAngle = Random.Range(0f, 360f);

        base.Start();
    }

    private void Update()
    {
        OrbitAroundPlayer();
        LookAtPlayer();

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OrbitAroundPlayer()
    {
        // Update the angle based on the orbit speed
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f; // Keep angle within 0-360 range
        }

        // Convert angle to radians for trigonometric functions
        float angleRad = currentAngle * Mathf.Deg2Rad;

        // Calculate the target position on the orbit
        Vector3 targetPosition = player.position + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * surroundingRadius;

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
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
