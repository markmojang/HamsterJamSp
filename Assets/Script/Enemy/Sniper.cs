using System.Collections;
using UnityEngine;

public class Sniper : Enemy
{
    public float rotationSpeed = 20f;
    public float attackRange = 40f;

    [SerializeField] private GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullet will be shot
    public float bulletSpeed = 40f; // Speed of the bullet
    public float fireRate = 2f; // Time between shots in seconds

    private float nextFireTime = 0f;
    private WaveManager waveManager;
    private int currentWave;

    protected override void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        currentWave = waveManager.currentWave - 1;
        float multiplier = 1f + (currentWave * 0.1f);
        health = 50f * multiplier;
        damage = 60f * multiplier;
        speed = 5f;

        // Create the object pool for enemy bullets
        ObjectPool.Instance.CreatePool("EnemyBullets", bulletPrefab, 10);

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
            // Move away from the player if too close
            Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
            transform.position += directionAwayFromPlayer * (speed * 2.25f) * Time.deltaTime;
            fireRate = 0.1f;
}
        else if (distance > attackRange)
        {
            // Move towards the player if too far
            Vector3 directionTowardsPlayer = (player.position - transform.position).normalized;
            transform.position += directionTowardsPlayer * speed* Time.deltaTime;
            fireRate = 2f;
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
