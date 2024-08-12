using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float damage;
    public GameObject hitEffect;

    void Start()
    {
        damage = GameObject.FindWithTag("Player").GetComponent<PlayerController>().Damage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damage);

            // Instantiate the hit effect at the collision point
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, col.transform.position, Quaternion.identity);
                Destroy(effect, 1f); // Adjust duration as needed
            }

            // Return the bullet to the pool
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // Return the bullet instance to the pool
        ObjectPool.Instance.ReturnObjectToPool("PlayerBullets", gameObject);
    }
}
