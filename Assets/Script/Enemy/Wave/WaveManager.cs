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
        StartCoroutine(StartWave());
    }

    public int GetCurrentwave()
    {
        return currentWave;
    }

}
