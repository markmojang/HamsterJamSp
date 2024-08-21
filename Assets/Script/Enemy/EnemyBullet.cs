using System.Collections;
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

            // Instantiate the hit effect at the collision point
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, collision.transform.position, Quaternion.identity);

                // Get the ParticleSystem component from the parent object only
                ParticleSystem parentParticleSystem = effect.GetComponent<ParticleSystem>();
                if (parentParticleSystem != null)
                {
                    // Access the MainModule of the parent ParticleSystem
                    var mainModule = parentParticleSystem.main;
                    Color randomHighSaturationColor = RandomHighSaturationColor();
                    mainModule.startColor = randomHighSaturationColor;
                }

                Destroy(effect, 1f); // Adjust duration as needed
            }

            if (cameraShake != null)
            {
                StartCoroutine(cameraShake.Shake(0.3f, 0.6f)); // Adjust duration and magnitude as needed
            }
            // Return the bullet to the pool
            ReturnToPool();
        }
    }
    private Color RandomHighSaturationColor()
    {
        // Create an array of three color channels, randomly set one to 1
        float[] colorChannels = new float[3];
        int maxIndex = Random.Range(0, 3);
        colorChannels[maxIndex] = 1f;

        // Randomly set the other two channels to 0 or 1
        for (int i = 0; i < 3; i++)
        {
            if (i != maxIndex)
            {
                colorChannels[i] = Random.value > 0.5f ? 1f : 0f;
            }
        }

        // Return the color
        return new Color(colorChannels[0], colorChannels[1], colorChannels[2]);
    }
    private void ReturnToPool()
    {
        // Return the bullet instance to the pool
        ObjectPool.Instance.ReturnObjectToPool("EnemyBullets", gameObject);
    }
}
