using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    private int enemiesAlive = 0;
    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponent<Spawner>();
        StartWave();
    }

    private void Update()
    {
        // Check if all enemies are killed
        if (enemiesAlive == 0)
        {
            StartNextWave();
        }
    }

    public void StartWave()
    {
        enemiesAlive = enemiesPerWave * currentWave;

        for (int i = 0; i < enemiesAlive; i++)
        {
            spawner.SpawnEnemy();
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
        StartWave();
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
            enemy.speed *= multiplier;
        }
    }
}
