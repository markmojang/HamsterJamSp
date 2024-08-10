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
        Debug.Log("Increasing difficulty: " + multiplier);

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Found " + enemyObjects.Length + " enemies");

        foreach (GameObject obj in enemyObjects)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.health *= multiplier;
                enemy.damage *= multiplier;
                Debug.Log("Updated enemy: " + obj.name + " | Health: " + enemy.health + " | Damage: " + enemy.damage);
            }
            else
            {
                Debug.Log("No Enemy component found on: " + obj.name);
            }
        }
    }

}
