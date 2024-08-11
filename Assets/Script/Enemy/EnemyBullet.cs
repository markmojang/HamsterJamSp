using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 0f;
    private Vector2 targetDirection;

    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);

            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.3f, 2f)); // Adjust duration and magnitude as needed
            }

            Destroy(gameObject, 3f);
        }
    }
}
