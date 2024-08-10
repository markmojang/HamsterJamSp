using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 0f;
    private Vector2 targetDirection;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
            Debug.Log(damage);
            Destroy(gameObject);
        }
    }
}
