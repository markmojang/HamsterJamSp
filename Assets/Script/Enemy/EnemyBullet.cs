using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 0f;
    public GameObject hitEffect; // Reference to the particle effect prefab

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

            // Instantiate the particle effect at the player's position
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
                Destroy(effect, 1f); // Adjust the duration as needed
            }

            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.3f, 0.6f)); // Adjust duration and magnitude as needed
            }

        }
    }
}
