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
    [SerializeField] float iFrameDuration = 1.5f; // Duration of invincibility in seconds
    [SerializeField] float blinkInterval = 0.15f;  // Interval between blinks
    
    [SerializeField] AudioSource HitSoundSource;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private bool isInvincible = false;

    void Start()
    {
        health = maxhp;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        HitSoundSource = gameObject.GetComponent<AudioSource>();
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
        if (!isInvincible)
        {
            HitSoundSource.Play();
            health -= amount;
            UpdateHealthBar();
            if (health <= 0)
            {
                Die();
            }
            StartCoroutine(BlinkAndInvincibility());
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

    private IEnumerator BlinkAndInvincibility()
    {
        isInvincible = true;
        playerCollider.enabled = false;

        float elapsedTime = 0f;

        while (elapsedTime < iFrameDuration)
        {
            // Toggle sprite visibility
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        // Ensure the sprite is visible after blinking ends
        spriteRenderer.enabled = true;
        playerCollider.enabled = true;
        isInvincible = false;
    }
}
