using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float damage;
    public float speed;
    protected Transform player;
    public AudioClip deathSFX; // Assign this through the Inspector
    public AudioClip hitSFX;
    private bool death = false;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        // Initialize with default stats or override in subclasses
    }

    public void TakeDamage(float amount)
    {
        if(!death){
            SoundManager.PlaySound(hitSFX);
            health -= amount;
            if (health <= 0)
            {
                Die();
                death = true;
            }
        }
    }

    protected virtual void Die()
    {
        // Play death sound effect using SoundManager
        if (deathSFX != null)
        {
            SoundManager.PlaySound(deathSFX);
        }

        // Notify WaveManager
        FindObjectOfType<WaveManager>().EnemyKilled();

        // Add currency to the player
        PlayerUpgrades playerUpgrades = FindObjectOfType<PlayerUpgrades>();
        if (playerUpgrades != null)
        {
            playerUpgrades.AddCurrency(1); // Add 1 currency
        }

        // Handle enemy death, e.g., play animation, remove from game
        Destroy(gameObject);
    }
}