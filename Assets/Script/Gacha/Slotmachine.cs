using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private AudioSource backgroundMusicSource;
    private AudioSource spinSoundSource;
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

        // Initialize AudioSources
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        spinSoundSource = gameObject.AddComponent<AudioSource>();

        // Set up background music source
        backgroundMusicSource.clip = backgroundClip;
        backgroundMusicSource.loop = false; // Enable looping
    }

    private void Update()
    {
        spinvalue.text = "SPIN X" + PlayerPrefs.GetInt("SpinPoint").ToString();
    }

    public void StartSpinning()
    {
        if (!isSpinning && PlayerPrefs.GetInt("SpinPoint") > 0)
        {
            PlayerPrefs.SetInt("SpinPoint", PlayerPrefs.GetInt("SpinPoint") - 1);
            for (int i = 0; i < slotImages.Length; i++)
            {
                Color imageColor = slotImages[i].color;
                imageColor.a = 1f; // Set alpha to 1 for fully opaque
                slotImages[i].color = imageColor;
            }
            // Play background music
            backgroundMusicSource.Play();

            StartCoroutine(SpinSlots());
        }
    }

    private IEnumerator SpinSlots()
    {
        isSpinning = true;
        int index = 0;
        float elapsed = 0f;
        float spriteChangeInterval = 0.1f; // Initial interval for sprite changes
        float nextSpriteChangeTime = 0f;

        while (elapsed <= spinDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            spinSpeed -= Time.unscaledDeltaTime; // Decelerate the spin speed

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

                    // Play the spin sound effect each time a sprite changes
                    if (spinClip)
                    {
                        spinSoundSource.PlayOneShot(spinClip);
                    }
                }

                // Increase the interval for the next sprite change
                spriteChangeInterval += 0.05f; // Adjust this value to control the deceleration rate
                nextSpriteChangeTime = Time.unscaledTime + spriteChangeInterval;
            }

            for (int i = 0 + index; i < slotImages.Length; i++)
            {
                // Move the slot image downward
                slotImages[i].rectTransform.anchoredPosition -= new Vector2(0, spinSpeed);

                // Loop the position if it goes off-screen
                if (slotImages[i].rectTransform.anchoredPosition.y < -50)
                {
                    slotImages[i].rectTransform.anchoredPosition = new Vector2(slotImages[i].rectTransform.anchoredPosition.x, 50);
                }
            }

            yield return null;
        }

        isSpinning = false;
        spinSpeed = spinSpeedfx;

        // Stop the background music when spinning stops
        backgroundMusicSource.Stop();
        CheckResult();
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
        playershoot.fireRate *= 2;
        playershoot.bulletSpeed /= 2;
    }

    private IEnumerator Spade()
    {
        player.moveSpeed *= 2;
        yield return new WaitForSeconds(4f);
        player.moveSpeed /= 2;
    }

    private IEnumerator Diamond()
    {
        player.Damage *= 2;
        yield return new WaitForSeconds(4f);
        player.Damage /= 2;
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
        else if (spinIndex == 1 && previousSymbol != null)
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
