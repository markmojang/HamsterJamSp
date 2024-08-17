using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public RectTransform inventoryPanel;

    [Tooltip("Duration of the panel transition animation.")]
    public float animationDuration = 1f;

    [Tooltip("Sound effect to play when the game is paused.")]
    public AudioClip pauseSound;

    [Tooltip("Sound effect to play when the game is resumed.")]
    public AudioClip resumeSound;

    private Vector2 leftPanelInitialPosition;
    private Vector2 rightPanelInitialPosition;
    private Vector2 leftPanelOffscreenPosition;
    private Vector2 rightPanelOffscreenPosition;
    private Vector2 inventoryInitialPosition;
    private Vector2 inventoryOffscreenPosition;
    private bool isPaused = false;
    private float animationProgress = 0f;
    private Coroutine currentAnimation = null;
    private AudioSource audioSource;
    private bool inv = false;

    void Start()
    {
        leftPanelInitialPosition = leftPanel.anchoredPosition;
        rightPanelInitialPosition = rightPanel.anchoredPosition;

        inventoryInitialPosition = inventoryPanel.anchoredPosition;
        inventoryOffscreenPosition = new Vector2(-Screen.width / 3, inventoryPanel.anchoredPosition.y);

        // Calculate off-screen positions
        leftPanelOffscreenPosition = new Vector2(-Screen.width / 3, leftPanel.anchoredPosition.y);
        rightPanelOffscreenPosition = new Vector2(Screen.width / 3, rightPanel.anchoredPosition.y);

        // Move panels off-screen initially
        leftPanel.anchoredPosition = leftPanelOffscreenPosition;
        rightPanel.anchoredPosition = rightPanelOffscreenPosition;
        inventoryPanel.anchoredPosition = inventoryOffscreenPosition;

        // Get or add an AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        // Play pause sound effect
        if (pauseSound != null)
        {
            audioSource.PlayOneShot(pauseSound);
        }
        currentAnimation = StartCoroutine(AnimatePanels(leftPanelOffscreenPosition, leftPanelInitialPosition, rightPanelOffscreenPosition, rightPanelInitialPosition));
    }

    private void ResumeGame()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        // Play resume sound effect
        if (resumeSound != null)
        {
            audioSource.PlayOneShot(resumeSound);
        }
        if(inv){
            inv = false;
            currentAnimation = StartCoroutine(AnimatePanelsedit(rightPanel, inventoryPanel, rightPanelInitialPosition, rightPanelOffscreenPosition , inventoryInitialPosition, inventoryOffscreenPosition));
        }
        else{
            currentAnimation = StartCoroutine(AnimatePanels(leftPanelInitialPosition, leftPanelOffscreenPosition, rightPanelInitialPosition, rightPanelOffscreenPosition));
        }
}

    public void Inventory_open(){
        currentAnimation = StartCoroutine(AnimatePanelsedit(leftPanel, inventoryPanel, leftPanelInitialPosition , leftPanelOffscreenPosition, inventoryOffscreenPosition, inventoryInitialPosition));
        inv = true;
    }
    private IEnumerator AnimatePanels(Vector2 leftStart, Vector2 leftEnd, Vector2 rightStart, Vector2 rightEnd)
    {
        while (animationProgress < animationDuration)
        {
            // Apply easing using Mathf.SmoothStep
            float t = Mathf.SmoothStep(0f, 1f, animationProgress / animationDuration);
            leftPanel.anchoredPosition = Vector2.Lerp(leftStart, leftEnd, t);
            rightPanel.anchoredPosition = Vector2.Lerp(rightStart, rightEnd, t);
            animationProgress += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure the panels are exactly in place
        leftPanel.anchoredPosition = leftEnd;
        rightPanel.anchoredPosition = rightEnd;
        animationProgress = 0f;

        if (isPaused)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
        else{
            isPaused = true;
        }

        currentAnimation = null;
    }
    private IEnumerator AnimatePanelsedit(RectTransform p1, RectTransform p2, Vector2 leftStart, Vector2 leftEnd, Vector2 rightStart, Vector2 rightEnd)
    {
        while (animationProgress < animationDuration)
        {
            // Apply easing using Mathf.SmoothStep
            float t = Mathf.SmoothStep(0f, 1f, animationProgress / animationDuration);
            p1.anchoredPosition = Vector2.Lerp(leftStart, leftEnd, t);
            p2.anchoredPosition = Vector2.Lerp(rightStart, rightEnd, t);
            animationProgress += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure the panels are exactly in place
        p1.anchoredPosition = leftEnd;
        p2.anchoredPosition = rightEnd;
        animationProgress = 0f;

        if(!inv){
            if(isPaused)
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                isPaused = false;
            }
            else{
                isPaused = true;
            }
        }

        currentAnimation = null;
    }
}
