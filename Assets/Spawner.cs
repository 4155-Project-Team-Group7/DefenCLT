using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyList;

    [Header("Attributes")]
    [SerializeField] private int baseNumEnemies = 8;
    [SerializeField] private float spawnRate = 0.5f;
    [SerializeField] private float timeUntilWave = 10f;
    [SerializeField] private float scalingFactor = 0.75f;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesRemaining;
    private bool isSpawning = false;

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (!isSpawning)
        { 
            return; 
        }
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= (1f / spawnRate) && enemiesRemaining > 0)
        {
            SpawnEnemy();
            enemiesRemaining--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
    }

    private void StartWave()
    {
        isSpawning = true;
        enemiesRemaining = EnemiesPerWave(); 
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyList[0];
        Instantiate(prefabToSpawn, LevelManager.main.start.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseNumEnemies * Mathf.Pow(currentWave, scalingFactor));
    }
}
