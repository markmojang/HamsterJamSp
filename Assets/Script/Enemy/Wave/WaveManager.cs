using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    public int checkpointInterval = 5; // Set the checkpoint interval
    private int checkpointWave = 1;
    private int enemiesAlive = 0;
    private Spawner spawner;
    private PlayerController player;
    [SerializeField] AudioSource WaveSoundSource;
    private WaveUI waveUI; // Reference to the WaveUI

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        WaveSoundSource = gameObject.GetComponent<AudioSource>();
        spawner = GetComponent<Spawner>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>(); 
        waveUI = FindObjectOfType<WaveUI>(); // Find the WaveUI in the scene

        // Load the checkpoint wave if it exists
        if (PlayerPrefs.HasKey("CheckpointWave"))
        {
            checkpointWave = PlayerPrefs.GetInt("CheckpointWave");
            currentWave = checkpointWave;
        }
        else
        {
            PlayerPrefs.SetInt("CheckpointWave", 1);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("SpinPoint"))
        {
            PlayerPrefs.SetInt("SpinPoint", 0);
            PlayerPrefs.Save();
        }
        
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        // Check if all enemies are killed
        if (enemiesAlive <= 0)
        {
            PlayerPrefs.SetInt("SpinPoint", PlayerPrefs.GetInt("SpinPoint") + 5);
            WaveSoundSource.Play();
            StartNextWave();
            Debug.Log("Next Wave");
        }
    }

    private IEnumerator StartWave()
    {
        enemiesAlive = enemiesPerWave + (currentWave - 1);
        for (int i = 0; i < enemiesPerWave + (currentWave - 1); i++)
        {
            spawner.SpawnEnemy();
            yield return new WaitForSeconds(0.7f); // Delay between each enemy spawn
        }

        // Update checkpoint if it's a checkpoint wave
        if (currentWave % checkpointInterval == 0)
        {
            checkpointWave = currentWave;
            PlayerPrefs.SetInt("CheckpointWave", checkpointWave); // Save the checkpoint wave
            PlayerPrefs.Save();
            Debug.Log("Checkpoint Reached at Wave " + checkpointWave);
        }
    }

    private void StartNextWave()
    {
        currentWave++;
        player.health = player.maxhp;
        player.UpdateHealthBar();

        // Trigger the "NEXT WAVE!!" text animation
        if (waveUI != null)
        {
            waveUI.ShowNextWaveText();
        }

        StartCoroutine(StartWave());
    }

    public void EnemyKilled()
    {
        enemiesAlive--;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetEnemiesRemaining()
    {
        return enemiesAlive;
    }

    // Method to reset the scene and start from the checkpoint
    public void ResetToCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        Debug.Log("Restarting from Checkpoint Wave " + checkpointWave);
    }
}
