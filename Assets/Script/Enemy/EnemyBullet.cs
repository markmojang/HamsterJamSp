using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 10f;
    private Vector3 direction;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction.normalized;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with player or environment
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit!!!!!!!!!");
            // other.GetComponent<Player>().TakeDamage(damage);
            // Destroy(gameObject);
        }
    }
}
