using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array of enemy prefabs to spawn
    public float spawnInterval = 5f;  // Time interval between spawns
    public float spawnRadius = 10f;  // Radius within which enemies will spawn
    public int maxEnemies = 10;  // Maximum number of active enemies

    private float timer;
    private int currentEnemyCount;

    private void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            timer = spawnInterval;  // Reset the timer
        }
    }

    private void SpawnEnemy()
    {
        // Choose a random position within the spawn radius
        Vector3 randomPosition = GetRandomPositionWithinRadius();

        // Choose a random enemy type from the array of enemyPrefabs
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        // Instantiate the enemy at the chosen spawn point
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        currentEnemyCount++;

        Debug.Log("Spawned an enemy of type: " + enemyPrefab.name + " at position: " + randomPosition);
    }

    private Vector3 GetRandomPositionWithinRadius()
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = transform.position.y; // Keep the Y position the same as the spawner
        return randomPosition;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}

