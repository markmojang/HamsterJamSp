using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxhp = 100;
    public float health;
    public float Damage = 100;
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    [SerializeField] private Image healthBar;

    void Start()
    {
        health = maxhp;
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        rb.velocity = movement * moveSpeed; 
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthBar();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You Die");
    }

    void UpdateHealthBar()
    {
        // Calculate the fill amount based on current health and max health
        float fillAmount = health / maxhp;
        healthBar.fillAmount = fillAmount;
    }
}
