using UnityEngine;

public class Ranger : Enemy
{
    public float rotationSpeed = 10f;
    public float surroundingRadius = 25f; // Desired distance from player
    public float orbitSpeed = 1f; // Speed at which enemies orbit around the player
    public float attackRange = 40f;
    public GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullets will be shot
    public float bulletSpeed = 20f; // Speed of the bullets
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
        speed = 10f;

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

    private Vector3 currentVelocity = Vector3.zero; // Used for SmoothDamp

    private void OrbitAroundPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > surroundingRadius)
        {
            // Move towards the player if too far from the attack range
            Vector3 directionTowardsPlayer = (player.position - transform.position).normalized;
            transform.position += directionTowardsPlayer * (speed * 2f) * Time.deltaTime;
        }

        // Update the angle based on the orbit speed
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f; // Keep angle within 0-360 range
        }

        // Convert angle to radians for trigonometric functions
        float angleRad = currentAngle * Mathf.Deg2Rad;

        // Calculate the target position on the orbit
        Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * surroundingRadius;
        Vector3 targetPosition = player.position + offset;

        // Check for nearby Rangers at similar positions
        Collider2D[] nearbyRangers = Physics2D.OverlapCircleAll(targetPosition, 1f);
        foreach (Collider2D rangerCollider in nearbyRangers)
        {
            Ranger otherRanger = rangerCollider.GetComponent<Ranger>();
            if (otherRanger != null && otherRanger != this)
            {
                // If the other Ranger is too close, adjust the position
                Vector3 directionAway = (transform.position - otherRanger.transform.position).normalized;
                targetPosition += directionAway * 1f; // Adjust this value for desired spacing
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 0.3f, speed);
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
