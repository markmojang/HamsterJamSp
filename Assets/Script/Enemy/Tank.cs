using System.Collections;
using UnityEngine;

public class Tank : Enemy
{
    public float rotationSpeed = 5f;
    public float minDistance = 1f;
    public float attackRange = 2f;

    [SerializeField] private GameObject bulletPrefab; // The enemy bullet prefab
    public Transform firePoint; // The fire point from where the bullets will be shot
    public float bulletSpeed = 10f; // Speed of the bullets
    public float fireRate = 3f; // Time between shots in seconds
    public int bulletCount = 5; // Number of bullets in the shotgun spread
    public float spreadAngle = 30f; // Total spread angle in degrees

    private float nextFireTime = 0f;
    private bool isAiming = true; // To ensure proper aiming before shooting
    private WaveManager waveManager;
    private int currentWave;

    protected override void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        currentWave = waveManager.currentWave - 1;
        float multiplier = 1f + (currentWave * 0.1f);
        health = 200f * multiplier;
        damage = 20f * multiplier;
        speed = 7f;

        // Create the object pool for enemy bullets
        ObjectPool.Instance.CreatePool("EnemyBullets", bulletPrefab, 10);

        base.Start();
    }

    private void FixedUpdate()
    {
        MaintainSpacing();
        MoveTowardsPlayer();
        LookAtPlayer();

        if (isAiming && Time.time >= nextFireTime)
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

        // Check if the Tank is facing the player
        if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) < 5f)
        {
            isAiming = true; // Ready to fire
        }
        else
        {
            isAiming = false; // Not ready to fire
        }
    }

    private void MaintainSpacing()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float spacingDistance = 10f; // Desired minimum distance between enemies

        foreach (GameObject enemy in enemies)
        {
            if (enemy == this.gameObject) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < spacingDistance)
            {
                // Calculate a direction away from the other enemy
                Vector3 directionAway = transform.position - enemy.transform.position;

                // Move slightly away to maintain spacing
                transform.position += directionAway.normalized * (spacingDistance - distance) * Time.deltaTime;
            }
        }
    }

    private void FireShotgun()
    {
        // Fire the first set of bullets
        FireShotgunSet();

        // Optional: Add a short delay between the two sets
        Invoke("FireShotgunSet", 0.2f); // Adjust the delay as needed
    }

    private void FireShotgunSet()
    {
        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, 0, currentAngle);

            GameObject bullet = ObjectPool.Instance.GetObjectFromPool("EnemyBullets");

            if (bullet != null)
            {
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = bulletRotation;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                EnemyBullet DMG = bullet.GetComponent<EnemyBullet>();
                DMG.damage = damage;

                if (rb != null)
                {
                    Vector3 direction = bulletRotation * Vector3.up;
                    rb.velocity = direction * bulletSpeed;
                }

                StartCoroutine(ReturnToPoolAfterDelay(bullet, 8f));
            }
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
