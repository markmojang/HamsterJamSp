using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    private int enemiesAlive = 0;
    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponent<Spawner>();
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
        
    }

    public void EnemyKilled()
    {
        enemiesAlive--;
    }

    private void StartNextWave()
    {
        currentWave++;
        IncreaseDifficulty();
        StartCoroutine(StartWave());
    }

    public void IncreaseDifficulty()
    {
        float multiplier = 1f + (currentWave * 0.1f);

        // Adjust enemy stats based on the wave
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.health *= multiplier;
            enemy.damage *= multiplier;
        }
    }
}
