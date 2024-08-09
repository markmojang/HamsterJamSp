using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public RectTransform leftPanel;
    public RectTransform rightPanel;

    private Vector2 leftPanelInitialPosition;
    private Vector2 rightPanelInitialPosition;
    private Vector2 leftPanelOffscreenPosition;
    private Vector2 rightPanelOffscreenPosition;
    private bool isPaused = false;
    private float animationDuration = 1f;

    void Start()
    {
        leftPanelInitialPosition = leftPanel.anchoredPosition;
        rightPanelInitialPosition = rightPanel.anchoredPosition;

        // Calculate off-screen positions
        leftPanelOffscreenPosition = new Vector2(-Screen.width /3, leftPanel.anchoredPosition.y);
        rightPanelOffscreenPosition = new Vector2(Screen.width /3, rightPanel.anchoredPosition.y);

        // Move panels off-screen initially
        leftPanel.anchoredPosition = leftPanelOffscreenPosition;
        rightPanel.anchoredPosition = rightPanelOffscreenPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC key pressed."); // Check if this is printed
            if (isPaused)
            {
                Debug.Log("Resuming game.");
                StartCoroutine(Resume());
            }
            else
            {
                Debug.Log("Pausing game.");
                StartCoroutine(Pause());
            }
            Debug.Log("Pressed");
        }
    }

    IEnumerator Pause()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;

        // Smoothly move the panels on screen
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            leftPanel.anchoredPosition = Vector2.Lerp(leftPanelOffscreenPosition, leftPanelInitialPosition, elapsedTime / animationDuration);
            rightPanel.anchoredPosition = Vector2.Lerp(rightPanelOffscreenPosition, rightPanelInitialPosition, elapsedTime / animationDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure the panels are exactly in place
        leftPanel.anchoredPosition = leftPanelInitialPosition;
        rightPanel.anchoredPosition = rightPanelInitialPosition;
    }

    IEnumerator Resume()
    {
        isPaused = false;

        // Smoothly move the panels off screen
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            leftPanel.anchoredPosition = Vector2.Lerp(leftPanelInitialPosition, leftPanelOffscreenPosition, elapsedTime / animationDuration);
            rightPanel.anchoredPosition = Vector2.Lerp(rightPanelInitialPosition, rightPanelOffscreenPosition, elapsedTime / animationDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ensure the panels are exactly off screen
        leftPanel.anchoredPosition = leftPanelOffscreenPosition;
        rightPanel.anchoredPosition = rightPanelOffscreenPosition;

        // Resume the game
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }
}
