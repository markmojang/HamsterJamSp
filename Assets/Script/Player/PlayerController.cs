using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this namespace

public class PlayerController : MonoBehaviour
{
    public float maxhp = 99999;
    public float health;
    public float Damage = 100;
    public float moveSpeed = 13;
    private Rigidbody2D rb;
    [SerializeField] private Image healthBar;
    [SerializeField] float iFrameDuration = 1.5f; // Duration of invincibility in seconds
    [SerializeField] float blinkInterval = 0.15f;  // Interval between blinks

    [SerializeField] AudioSource HitSoundSource;
    private WaveManager waveManager;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private bool isInvincible = false;

    // Add these fields
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text fireRateText;
     [SerializeField] private TMP_Text agText;
    [SerializeField] private TMP_Text bcText; // Add this field

    private PlayerShooter playerShooter; // Add this field

    void Start()
    {
        maxhp = PlayerPrefs.GetFloat("PmaxHp");
        health = PlayerPrefs.GetFloat("PMoveSpeed");
        Damage = PlayerPrefs.GetFloat("PDamage");
        moveSpeed = PlayerPrefs.GetFloat("PMoveSpeed");
        health = maxhp;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        HitSoundSource = gameObject.GetComponent<AudioSource>();
        waveManager = FindObjectOfType<WaveManager>();

        // Find the PlayerShooter component on the child GameObject
        playerShooter = GetComponentInChildren<PlayerShooter>();

        if (playerShooter == null)
        {
            Debug.LogError("PlayerShooter component not found on the GameObject.");
        }

        // Initialize UI texts
        UpdateStatUI();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        rb.velocity = movement * moveSpeed;

        // Update the UI with the current stats
        UpdateStatUI();
        UpdateHealthBar();
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
        waveManager.ResetToCheckpoint();
    }

    public void UpdateHealthBar()
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

    private void UpdateStatUI()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + health.ToString("F0") + "/" + maxhp.ToString("F0");
        }
        if (damageText != null)
        {
            damageText.text = "DMG: " + Damage.ToString("F0");
        }
        if (speedText != null)
        {
            speedText.text = "SPD: " + moveSpeed.ToString("F0");
        }
        if (fireRateText != null && playerShooter != null)
        {
            fireRateText.text = "FIR: " + playerShooter.fireRate.ToString("F2");
        }
         if (bcText != null)
        {
            bcText.text = "BC: " + playerShooter.bulletCount.ToString("F0");
        }
         if (agText != null)
        {
            agText.text = "AG: " + playerShooter.spreadAngle.ToString("F0");
        }
        else
        {
            Debug.LogWarning("Fire rate text or PlayerShooter reference is missing.");
        }
    }
}
