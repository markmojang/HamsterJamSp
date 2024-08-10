using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float damage;
    public float speed;
    protected Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        // Initialize with default stats or override in subclasses
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        // Notify WaveManager
        FindObjectOfType<WaveManager>().EnemyKilled();

        // Handle enemy death, e.g., play animation, remove from game
        Destroy(gameObject);
    }
    
    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
