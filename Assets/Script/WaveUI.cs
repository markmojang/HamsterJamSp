using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    public TMP_Text waveText; // Reference to the TextMeshPro Text element for wave
    public TMP_Text enemiesRemainingText; // Reference to the TextMeshPro Text element for remaining enemies
    private WaveManager waveManager; // Reference to the WaveManager

    private void Start()
    {
        // Find the WaveManager in the scene
        waveManager = FindObjectOfType<WaveManager>();

        // Update the UI at the start
        UpdateWaveText();
        UpdateEnemiesRemainingText();
    }

    private void Update()
    {
        // Continuously update the UI if the current wave or enemy count changes
        if (waveManager != null)
        {
            UpdateWaveText();
            UpdateEnemiesRemainingText();
        }
    }

    private void UpdateWaveText()
    {
        if (waveText != null && waveManager != null)
        {
            waveText.text = "Wave: " + waveManager.GetCurrentWave().ToString();
        }
    }

    private void UpdateEnemiesRemainingText()
    {
        if (enemiesRemainingText != null && waveManager != null)
        {
            enemiesRemainingText.text = "Enemies Remaining: " + waveManager.GetEnemiesRemaining().ToString();
        }
    }
}
