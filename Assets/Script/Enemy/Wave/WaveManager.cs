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

    private void Awake()
    {
        # PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        spawner = GetComponent<Spawner>();

        // Load the checkpoint wave if it exists
        if (PlayerPrefs.HasKey("CheckpointWave"))
        {
            checkpointWave = PlayerPrefs.GetInt("CheckpointWave");
            currentWave = checkpointWave;
        }
        
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        // Check if all enemies are killed
        if (enemiesAlive <= 0)
        {
            StartNextWave();
            Debug.Log("Next Wave");
        }
    }

    private IEnumerator StartWave()
    {
        enemiesAlive = enemiesPerWave * currentWave;
        for (int i = 0; i < (enemiesPerWave * currentWave); i++)
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

    public void EnemyKilled()
    {
        enemiesAlive--;
    }

    private void StartNextWave()
    {
        currentWave++;
        StartCoroutine(StartWave());
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
