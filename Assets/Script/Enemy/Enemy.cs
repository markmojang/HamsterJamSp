using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float damage;
    public float speed;
    protected IAttackPattern attackPattern;
    protected Transform player;

    protected virtual void Start()
    {
        // Initialize with default stats or override in subclasses
    }

    public virtual void Attack()
    {
        attackPattern.ExecuteAttack(this);
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
}
