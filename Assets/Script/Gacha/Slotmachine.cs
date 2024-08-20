using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Slotmachine : MonoBehaviour
{
    public Image[] slotImages; // Array to hold the images representing the slots
    public Sprite[] slotSprites; // Array to hold the possible sprites for the slots
    [SerializeField] private float spinSpeedfx;
    [SerializeField] private float delaystop = 0.5f;
    public float spinDuration = 2f; // How long the slots should spin for
    [SerializeField] private float spinSpeed; // Speed of the slot spinning
    private bool isSpinning = false;
    public List<Vector2> initialPosition = new List<Vector2>(); // Use a List instead of an array
    [SerializeField] private TMP_Text spinvalue;
    [SerializeField] private AudioClip spinClip; // The sound effect that plays during each spin
    [SerializeField] private AudioClip backgroundClip; // The background sound that plays during spinning

    [SerializeField] private float autoSpinDelay = 0.5f; // Delay between spins
    private bool isAutoSpinning = false;
    private Coroutine autoSpinCoroutine;

    [SerializeField] private float spinVolume = 1.0f; // Volume for the spin sound effect
    [SerializeField] private float backgroundVolume = 1.0f; // Volume for the background music

    private AudioSource audioSource;
    private PlayerController player;
    private PlayerShooter playershoot;
    private string previousSymbol = null;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playershoot = GameObject.FindWithTag("Player").transform.GetChild(0).gameObject.GetComponent<PlayerShooter>();
        spinSpeed = spinSpeedfx;
        for (int i = 0; i < slotImages.Length; i++)
        {
            Color imageColor = slotImages[i].color;
            imageColor.a = 1f;
            initialPosition.Add(slotImages[i].rectTransform.anchoredPosition); // Use Add() to add elements to the list
        }

        // Initialize AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = spinVolume; // Set volume for spin sound

        // Set up background music
        audioSource.clip = backgroundClip;
        audioSource.loop = true; // Enable looping for background music
        audioSource.volume = backgroundVolume; // Set volume for background music
    }

    private void Update()
    {
        spinvalue.text = "SPIN X" + PlayerPrefs.GetInt("SpinPoint").ToString();
    }

    public void StartAutoSpin()
    {
        if (isAutoSpinning)
        {
            StopAutoSpin();
            return;
        }

        if (!isSpinning && PlayerPrefs.GetInt("SpinPoint") > 0)
        {
            isAutoSpinning = true;
            autoSpinCoroutine = StartCoroutine(AutoSpin());
        }
    }

    private IEnumerator AutoSpin()
    {
        // Keep spinning until spin points are exhausted or auto spinning is stopped
        while (PlayerPrefs.GetInt("SpinPoint") > 0 && isAutoSpinning)
        {
            StartSpinning(); // Start a single spin
            yield return new WaitWhile(() => isSpinning); // Wait until the spin is done
            yield return new WaitForSecondsRealtime(autoSpinDelay); // Use unscaled time for delay
        }

        isAutoSpinning = false;
        Debug.Log("Auto spin finished or stopped.");
    }

    public void StopAutoSpin()
    {
        if (isAutoSpinning)
        {
            isAutoSpinning = false;
            if (autoSpinCoroutine != null)
            {
                StopCoroutine(autoSpinCoroutine);
                autoSpinCoroutine = null;
            }
            Debug.Log("Auto spin stopped by user.");
        }
    }

    public void StartSpinning()
    {
        if (!isSpinning && PlayerPrefs.GetInt("SpinPoint") > 0)
        {
            isSpinning = true; // Indicate that spinning has started
            PlayerPrefs.SetInt("SpinPoint", PlayerPrefs.GetInt("SpinPoint") - 1); // Reduce spin points

            for (int i = 0; i < slotImages.Length; i++)
            {
                Color imageColor = slotImages[i].color;
                imageColor.a = 1f;
                slotImages[i].color = imageColor;
            }

            audioSource.clip = backgroundClip;
            audioSource.Play(); // Play background music
            StartCoroutine(SpinSlots());
        }
    }

    private IEnumerator SpinSlots()
    {
        isSpinning = true;
        float elapsed = 0f;
        int index = 0;
        float spriteChangeInterval = 0.1f;
        float nextSpriteChangeTime = 0f;

        while (elapsed <= spinDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time here
            spinSpeed -= Time.unscaledDeltaTime; // Adjust spin speed with unscaled time

            if (elapsed >= spinDuration && index == 2)
            {
                string chosenSymbol = ChooseSymbol(index);
                int finalIndex = GetSpriteIndex(chosenSymbol);
                slotImages[index].sprite = slotSprites[finalIndex];
                slotImages[index].rectTransform.anchoredPosition = initialPosition[index];
            }
            else if (elapsed >= spinDuration - delaystop && index == 1)
            {
                string chosenSymbol = ChooseSymbol(index);
                int finalIndex = GetSpriteIndex(chosenSymbol);
                slotImages[index].sprite = slotSprites[finalIndex];
                slotImages[index].rectTransform.anchoredPosition = initialPosition[index];
                index++;
            }
            else if (elapsed >= spinDuration - delaystop * 2 && index == 0)
            {
                string chosenSymbol = ChooseSymbol(index);
                int finalIndex = GetSpriteIndex(chosenSymbol);
                slotImages[index].sprite = slotSprites[finalIndex];
                slotImages[index].rectTransform.anchoredPosition = initialPosition[index];
                index++;
            }

            if (Time.unscaledTime > nextSpriteChangeTime)
            {
                for (int i = 0 + index; i < slotImages.Length; i++)
                {
                    string chosenSymbol = ChooseSymbol(i);
                    int randomIndex = GetSpriteIndex(chosenSymbol);
                    slotImages[i].sprite = slotSprites[randomIndex];
                    audioSource.PlayOneShot(spinClip); // Play spin sound
                }
                spriteChangeInterval += 0.05f;
                nextSpriteChangeTime = Time.unscaledTime + spriteChangeInterval;
            }

            for (int i = 0 + index; i < slotImages.Length; i++)
            {
                slotImages[i].rectTransform.anchoredPosition -= new Vector2(0, spinSpeed);
                if (slotImages[i].rectTransform.anchoredPosition.y < -50)
                {
                    slotImages[i].rectTransform.anchoredPosition = new Vector2(slotImages[i].rectTransform.anchoredPosition.x, 50);
                }
            }

            yield return null;
        }

        isSpinning = false;
        spinSpeed = spinSpeedfx;
        audioSource.Stop(); // Stop background music

        CheckResult();
    }

    public void SellSlot()
    {
        if (PlayerPrefs.GetInt("SpinPoint") > 0)
        {
            // Find GameObject with tag "Player" and get the component
            PlayerUpgrades player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUpgrades>();
            if (player != null)
            {
                player.AddCurrency(PlayerPrefs.GetInt("SpinPoint"));
            }

            // Find GameObject with tag "Wave" and get the component
            WaveManager wave = GameObject.FindGameObjectWithTag("Wave").GetComponent<WaveManager>();
            if (wave != null)
            {
                wave.PlayWaveSound();
            }

            // Set SpinPoint to 0
            PlayerPrefs.SetInt("SpinPoint", 0);
        }
    }

    private void CheckResult()
    {
        if (slotImages[0].sprite.name == slotImages[1].sprite.name && slotImages[1].sprite.name == slotImages[2].sprite.name)
        {
            switch (slotImages[0].sprite.name)
            {
                case "Clover":
                    Debug.Log("Run " + slotImages[0].sprite.name + " Function");
                    StartCoroutine(Clover());
                    break;
                case "Spade":
                    Debug.Log("Run " + slotImages[0].sprite.name + " Function");
                    StartCoroutine(Spade());
                    break;
                case "Diamond":
                    Debug.Log("Run " + slotImages[0].sprite.name + " Function");
                    StartCoroutine(Diamond());
                    break;
                case "Heart":
                    Debug.Log("Run " + slotImages[0].sprite.name + " Function");
                    StartCoroutine(Heart());
                    break;
            }
        }
    }

    private IEnumerator Clover()
    {
        playershoot.fireRate /= 2;
        playershoot.bulletSpeed *= 2;
        yield return new WaitForSeconds(4f);
        playershoot.fireRate = PlayerPrefs.GetFloat("PFirerate");
        playershoot.bulletSpeed = PlayerPrefs.GetFloat("PVelocity");
    }

    private IEnumerator Spade()
    {
        player.moveSpeed *= 2;                   // Double the player's speed
        yield return new WaitForSeconds(4f);     // Wait for 4 seconds
        player.moveSpeed = PlayerPrefs.GetFloat("PMoveSpeed");        // Restore the original speed
    }

    

    private IEnumerator Diamond()
    {
        float originalDamage = player.Damage;
        player.Damage *= 2;
        yield return new WaitForSeconds(4f);
        player.Damage = PlayerPrefs.GetFloat("PDamage");
    }

    private IEnumerator Heart()
    {
        player.health = player.maxhp;
        yield return null;
    }

    // Helper function to choose a symbol based on the previous one
    private string ChooseSymbol(int spinIndex)
    {
        string[] symbols = new string[] { "Clover", "Spade", "Diamond", "Heart" };

        if (spinIndex == 0)
        {
            // For the first spin, choose a symbol randomly
            previousSymbol = symbols[Random.Range(0, symbols.Length)];
            return previousSymbol;
        }
        else if (spinIndex >= 1 && previousSymbol != null)
        {
            // For the second spin, if the first symbol was something, give 50% chance to get the same symbol again
            if (Random.value < 0.5f)
            {
                return previousSymbol;
            }
            else
            {
                // 50% chance to get one of the other symbols
                List<string> otherSymbols = new List<string>(symbols);
                otherSymbols.Remove(previousSymbol);
                return otherSymbols[Random.Range(0, otherSymbols.Count)];
            }
        }
        else
        {
            // For the third spin, or when previous symbol is not relevant, choose randomly
            previousSymbol = null;
            return symbols[Random.Range(0, symbols.Length)];
        }
    }

    // Helper function to get the index of a symbol in the sprite array
    private int GetSpriteIndex(string symbol)
    {
        for (int i = 0; i < slotSprites.Length; i++)
        {
            if (slotSprites[i].name == symbol)
            {
                return i;
            }
        }
        return 0; // Default to the first sprite if not found
    }
}
