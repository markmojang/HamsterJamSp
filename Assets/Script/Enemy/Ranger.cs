using System.Collections;
using UnityEngine;

public class Ranger : Enemy
{
    public float rotationSpeed = 10f;
    public float surroundingRadius = 20f; // Desired distance from player
    public float orbitSpeed = 100f; // Speed at which enemies orbit around the player
    public float attackRange = 40f;

    [SerializeField] private GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullets will be shot
    public float bulletSpeed = 20f; // Speed of the bullets
    public float fireRate = 0.2f; // Time between shots in seconds

    private float nextFireTime = 0f;
    private WaveManager waveManager;
    private int currentWave;
    private float currentAngle;

    private enum RangerState
    {
        Orbiting,
        SpeedingTowardsPlayer
    }

    private RangerState currentState = RangerState.Orbiting;

    protected override void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        currentWave = waveManager.currentWave - 1;
        float multiplier = 1f + (currentWave * 0.1f);
        health = 100f * multiplier;
        damage = 30f * multiplier;
        speed = 12f;

        // Assign a random initial angle for this enemy
        currentAngle = Random.Range(0f, 360f);

        // Create the object pool for enemy bullets
        ObjectPool.Instance.CreatePool("EnemyBullets", bulletPrefab, 10);

        base.Start();
    }

    private void FixedUpdate()
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
        float distance = Vector3.Distance(transform.position, player.position);

        // Determine the state based on the distance to the player
        currentState = distance > surroundingRadius ? RangerState.SpeedingTowardsPlayer : RangerState.Orbiting;

        Vector3 targetPosition = transform.position;

        switch (currentState)
        {
            case RangerState.SpeedingTowardsPlayer:
                Vector3 directionTowardsPlayer = (player.position - transform.position).normalized;
                targetPosition = directionTowardsPlayer * (speed * 2.25f) * Time.deltaTime;
                break;

            case RangerState.Orbiting:
                currentAngle = (currentAngle + orbitSpeed * Time.deltaTime) % 360f;
                float angleRad = currentAngle * Mathf.Deg2Rad;
                targetPosition = player.position + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * surroundingRadius;
                break;
        }

        // Check for nearby Rangers at similar positions and adjust the position if necessary
        Collider2D[] nearbyRangers = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (Collider2D rangerCollider in nearbyRangers)
        {
            Ranger otherRanger = rangerCollider.GetComponent<Ranger>();
            if (otherRanger != null && otherRanger != this)
            {
                float otherRangerDistance = Vector3.Distance(transform.position, otherRanger.transform.position);
                if (otherRangerDistance < 10f)
                {
                    Vector3 directionAway = (transform.position - otherRanger.transform.position).normalized;
                    targetPosition += directionAway * (10f - otherRangerDistance);
                }
            }
        }

        // Smoothly move the ranger towards the target position
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
        GameObject bullet = ObjectPool.Instance.GetObjectFromPool("EnemyBullets");

        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            EnemyBullet DMG = bullet.GetComponent<EnemyBullet>();
            DMG.damage = damage;

            if (rb != null)
            {
                Vector3 direction = (player.position - firePoint.position).normalized;
                rb.velocity = direction * bulletSpeed;
            }

            StartCoroutine(ReturnToPoolAfterDelay(bullet, 8f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject bullet, float delay)
    {
        float elapsedTime = 0f;
        bool checkes = false;
        while (elapsedTime <= delay)
        {
            if (!bullet.activeSelf) // Check if the GameObject is inactive
            {
                checkes = true;
                break;
            }
            else
            {
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }
        if(!checkes){
            ObjectPool.Instance.ReturnObjectToPool("EnemyBullets", bullet);
        }
    }
}
