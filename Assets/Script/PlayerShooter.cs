using System.Collections;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;  // Serialized for inspector assignment

    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float firect = 0f;
    private Camera mainCamera;
    private int time = 1;
    [SerializeField] private float bulletlifespan;

    void Start()
    {
        fireRate = PlayerPrefs.GetFloat("PFirerate");
        bulletSpeed = PlayerPrefs.GetFloat("PVelocity");
        mainCamera = Camera.main;

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
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - transform.position).normalized;

        GameObject bullet = ObjectPool.Instance.GetObjectFromPool("PlayerBullets");

        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;

            StartCoroutine(vanishbullet(bullet));
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
                elapsedTime = 0f; // Reset the timer
                Debug.Log("Reset Timer " + time.ToString());
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
            Debug.Log("Returned " + time.ToString());
            ObjectPool.Instance.ReturnObjectToPool("PlayerBullets", objects);
            time++;
        }
        
    }
}
