using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ถ้าใช้ UI Image
public class Slotmachine : MonoBehaviour
{
    public Image[] slotImages; // Array to hold the images representing the slots
    public Sprite[] slotSprites; // Array to hold the possible sprites for the slots
    [SerializeField] private float spinSpeedfx;
    [SerializeField] private float delaystop = 0.5f;
    public float spinDuration = 2f; // How long the slots should spin for
    
    [SerializeField]private float spinSpeed; // Speed of the slot spinning
    private bool isSpinning = false;
    public List<Vector2> initialPosition = new List<Vector2>(); // Use a List instead of an array

    void Start()
    {
        spinSpeed = spinSpeedfx;
        for (int i = 0; i < slotImages.Length; i++)
        {
            initialPosition.Add(slotImages[i].rectTransform.anchoredPosition); // Use Add() to add elements to the list
        }
    }


    public void StartSpinning()
    {
        if (!isSpinning)
        {
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

        while (elapsed < spinDuration + delaystop*2)
        {
            elapsed += Time.unscaledDeltaTime;
            spinSpeed -= Time.unscaledDeltaTime; // Decelerate the spin speed

            // Change sprites only at intervals, with decreasing frequency
            if(elapsed >= spinDuration+delaystop*2 && index == 2){
                int finalIndex = Random.Range(0, slotSprites.Length);
                slotImages[index].sprite = slotSprites[finalIndex];
                slotImages[index].rectTransform.anchoredPosition = initialPosition[index];
            }
            else if(elapsed >= spinDuration+delaystop && index == 1){
                int finalIndex = Random.Range(0, slotSprites.Length);
                slotImages[index].sprite = slotSprites[finalIndex];
                slotImages[index].rectTransform.anchoredPosition = initialPosition[index];
                index++;
            }
            else if(elapsed >= spinDuration && index == 0){
                int finalIndex = Random.Range(0, slotSprites.Length);
                slotImages[index].sprite = slotSprites[finalIndex];
                slotImages[index].rectTransform.anchoredPosition = initialPosition[index];
                index++;
            }
            if (Time.unscaledTime > nextSpriteChangeTime)
            {
                for (int i = 0+index; i < slotImages.Length; i++)
                {
                    int randomIndex = Random.Range(0, slotSprites.Length);
                    slotImages[i].sprite = slotSprites[randomIndex];
                }

                // Increase the interval for the next sprite change
                spriteChangeInterval += 0.05f; // Adjust this value to control the deceleration rate
                nextSpriteChangeTime = Time.unscaledTime + spriteChangeInterval;
            }

            for (int i = 0+index; i < slotImages.Length; i++)
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

        for (int i = 0; i < slotImages.Length; i++)
        {
            int finalIndex = Random.Range(0, slotSprites.Length);
            slotImages[i].sprite = slotSprites[finalIndex];

            // Reset the position to the initial position
            slotImages[i].rectTransform.anchoredPosition = initialPosition[i];
            yield return new WaitForSecondsRealtime(0.5f);
        }

        isSpinning = false;
        spinSpeed = spinSpeedfx;
    }
}


