using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int currentWave = 1;
    public int enemiesPerWave = 5;
    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponent<Spawner>();
        StartWave();
    }

    public void StartWave()
    {
        int maxEnemies = enemiesPerWave * currentWave;
        for (int i = 0; i < maxEnemies; i++)
        {
            spawner.SpawnEnemy();
        }
    }

    public void IncreaseDifficulty()
    {
        currentWave++;
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
