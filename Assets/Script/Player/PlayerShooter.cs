using System.Collections;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;  // Serialized for inspector assignment
    public float spreadAngle = 30f; 
    public int bulletCount = 5;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float firect = 0f;
    private Camera mainCamera;
    [SerializeField] private float bulletlifespan;

    void Start()
    {
        fireRate = PlayerPrefs.GetFloat("PFirerate");
        bulletSpeed = PlayerPrefs.GetFloat("PVelocity");
        mainCamera = Camera.main;
        bulletCount = PlayerPrefs.GetInt("PBulletCount");
        spreadAngle = PlayerPrefs.GetFloat("PSpreadAngle");

        ObjectPool.Instance.CreatePool("PlayerBullets", bulletPrefab, 10);  // Initialize the pool with player bullets
    }

    void Update()
    {
        if (firect < fireRate)
        {
            firect += Time.deltaTime;
        }
        else
        {
            Shoot();
            firect = 0f;
        }
    }


void Shoot()
{
    if (bulletCount == 1)
    {
        // Get the mouse position and convert it to world space
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction from the player to the mouse position
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Get a bullet from the pool
        GameObject bullet = ObjectPool.Instance.GetObjectFromPool("PlayerBullets");

        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed; // Assuming bulletSpeed is set elsewhere
            }

            StartCoroutine(vanishbullet(bullet));
        }
    }
    else {
    // Ensure bulletCount is at least 1
    bulletCount = Mathf.Max(bulletCount, 1);

    // Get the mouse position and convert it to world space
    Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0f;

    // Calculate the base direction from the player to the mouse position
    Vector3 baseDirection = (mousePosition - transform.position).normalized;

    // Calculate spread parameters
    float angleStep = spreadAngle / (bulletCount - 1);
    float startAngle = -spreadAngle / 2;

    for (int i = 0; i < bulletCount; i++)
    {
        // Calculate the current angle and create a rotation based on that angle
        float currentAngle = startAngle + (i * angleStep);
        Quaternion bulletRotation = Quaternion.Euler(0, 0, currentAngle);

        // Apply the rotation to the base direction
        Vector3 directionWithSpread = bulletRotation * baseDirection;

        // Get a bullet from the pool
        GameObject bullet = ObjectPool.Instance.GetObjectFromPool("PlayerBullets");

        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionWithSpread);

            // Set the bullet's velocity
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = directionWithSpread * bulletSpeed;
            }

            // Start the coroutine to return the bullet to the pool
            StartCoroutine(vanishbullet(bullet));
            }
        }
    }
}


    IEnumerator vanishbullet(GameObject objects)
    {
        float elapsedTime = 0f;
        bool checkes = false;
        while (elapsedTime <= bulletlifespan)
        {
            if (!objects.activeSelf) // Check if the GameObject is inactive
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
            ObjectPool.Instance.ReturnObjectToPool("PlayerBullets", objects);
        }
        
    }
}
