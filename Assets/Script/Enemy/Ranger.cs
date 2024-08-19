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

    public float transitionCooldownTime = 1f; // Time in seconds before allowing state switch
    private float transitionCooldownTimer;
    private enum RangerState
    {
        Orbiting,
        SpeedingTowardsPlayer
    }

    private RangerState currentState = RangerState.Orbiting;

    protected override void Start()
    {
        currentState = RangerState.SpeedingTowardsPlayer; // Initial state
        transitionCooldownTimer = 0f;

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
        float distance = Vector3.Distance(transform.position, player.position);
        // Increment the state timer
        if (transitionCooldownTimer > 0f)
        {
            transitionCooldownTimer -= Time.fixedDeltaTime;
        }

        // Transition between states based on distance to player and cooldown
        if (currentState == RangerState.Orbiting && distance > (surroundingRadius + 5f) && transitionCooldownTimer <= 0f)
        {
            ChangeState(RangerState.SpeedingTowardsPlayer);
        }
        else if (currentState == RangerState.SpeedingTowardsPlayer && distance <= (surroundingRadius + 5f) && transitionCooldownTimer <= 0f)
        {
            ChangeState(RangerState.Orbiting);
        }

        // Execute the current state's behavior
        ExecuteState();
        LookAtPlayer();

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void ChangeState(RangerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            transitionCooldownTimer = transitionCooldownTime; // Reset the cooldown timer whenever a state is changed
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case RangerState.SpeedingTowardsPlayer:
                SpeedTowardsPlayer();
                break;

            case RangerState.Orbiting:
                OrbitAround();
                break;
        }

        AdjustPositionToAvoidNearbyRangers();
    }

    private void SpeedTowardsPlayer()
    {
        Vector3 directionTowardsPlayer = (player.position - transform.position).normalized;
        transform.position += directionTowardsPlayer * (speed * 1.5f) * Time.fixedDeltaTime;
    }

    private void OrbitAround()
    {
        currentAngle = (currentAngle + orbitSpeed * Time.fixedDeltaTime) % 360f;
        float angleRad = currentAngle * Mathf.Deg2Rad;
        Vector3 targetPosition = player.position + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * surroundingRadius;

        // Smoothly move the ranger towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
    }

    private void AdjustPositionToAvoidNearbyRangers()
    {
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
                    transform.position += directionAway * (10f - otherRangerDistance);
                }
            }
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
