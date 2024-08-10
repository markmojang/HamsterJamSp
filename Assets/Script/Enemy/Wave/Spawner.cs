using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRadius = 10f;
    public Transform player;

    public GameObject tankPrefab;
    public GameObject rangerPrefab;
    public GameObject sniperPrefab;

    public void SpawnEnemy()
    {
        Vector2 spawnPosition = (Vector2)player.position + (Random.insideUnitCircle.normalized * spawnRadius);
        GameObject enemyPrefab = ChooseEnemyPrefab();  // Implement this method to randomly choose an enemy type
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private GameObject ChooseEnemyPrefab()
    {
        // Logic to choose which enemy to spawn (Tank, Ranger, or Sniper)
        // For example, using a random selection:
        int rand = Random.Range(0, 3);  // Adjust based on your enemy types
        switch (rand)
        {
            case 0:
                return tankPrefab;
            case 1:
                return rangerPrefab;
            case 2:
                return sniperPrefab;
            default:
                return null;
        }
    }
}
