using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    public TMP_Text waveText; // Reference to the TextMeshPro Text element for wave
    public TMP_Text enemiesRemainingText; // Reference to the TextMeshPro Text element for remaining enemies
    public TMP_Text zoomText; // Reference to the TextMeshPro Text element for camera zoom

    private WaveManager waveManager; // Reference to the WaveManager
    private CameraController cameraController; // Reference to the CameraController

    private void Start()
    {
        // Find the WaveManager and CameraController in the scene
        waveManager = FindObjectOfType<WaveManager>();
        cameraController = FindObjectOfType<CameraController>();

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
}
