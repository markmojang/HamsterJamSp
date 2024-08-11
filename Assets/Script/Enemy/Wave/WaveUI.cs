using UnityEngine;
using TMPro;
using System.Collections;

public class WaveUI : MonoBehaviour
{
    public TMP_Text waveText; // Reference to the TextMeshPro Text element for wave
    public TMP_Text enemiesRemainingText; // Reference to the TextMeshPro Text element for remaining enemies
    public TMP_Text zoomText; // Reference to the TextMeshPro Text element for camera zoom
    public TMP_Text nextWaveText; // Reference to the TextMeshPro Text element for "NEXT WAVE!!"

    private WaveManager waveManager; // Reference to the WaveManager
    private CameraController cameraController; // Reference to the CameraController

    private void Start()
    {
        // Find the WaveManager and CameraController in the scene
        waveManager = FindObjectOfType<WaveManager>();
        cameraController = FindObjectOfType<CameraController>();

        // Hide the "NEXT WAVE!!" text at the start
        if (nextWaveText != null)
        {
            nextWaveText.alpha = 0f;
        }

        // Update the UI at the start
        UpdateWaveText();
        UpdateEnemiesRemainingText();
        UpdateZoomText();
    }

    private void Update()
    {
        // Continuously update the UI if the current wave, enemy count, or camera zoom changes
        if (waveManager != null)
        {
            UpdateWaveText();
            UpdateEnemiesRemainingText();
        }

        if (cameraController != null)
        {
            UpdateZoomText();
        }
    }

    private void UpdateWaveText()
    {
        if (waveText != null && waveManager != null)
        {
            waveText.text = "WAVE: " + waveManager.GetCurrentWave().ToString();
        }
    }

    private void UpdateEnemiesRemainingText()
    {
        if (enemiesRemainingText != null && waveManager != null)
        {
            enemiesRemainingText.text = "ENEMIES: " + waveManager.GetEnemiesRemaining().ToString();
        }
    }

    private void UpdateZoomText()
    {
        if (zoomText != null && cameraController != null)
        {
            float currentZoom = cameraController.Cam.orthographic ? cameraController.Cam.orthographicSize : cameraController.Cam.fieldOfView;
            currentZoom = 5/currentZoom;
            zoomText.text = "ZOOM " + currentZoom.ToString("F1") + "X";
        }
    }

    // Method to trigger the "NEXT WAVE!!" text animation
    public void ShowNextWaveText()
    {
        if (nextWaveText != null)
        {
            StartCoroutine(FadeTextInAndOut(nextWaveText, 0.5f, 0.5f)); // 1 second to fade in and out
        }
    }

    // Coroutine to fade the text in and out
    private IEnumerator FadeTextInAndOut(TMP_Text text, float fadeInTime, float fadeOutTime)
    {
        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Clamp01(elapsedTime / fadeInTime);
            yield return null;
        }

        // Wait for a short duration before starting to fade out
        yield return new WaitForSeconds(1f);

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeOutTime));
            yield return null;
        }

        // Ensure the text is fully invisible after fading out
        text.alpha = 0f;
    }
}
