using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    [Header("References")]
    [SerializeField] private GameObject[] enemyList;

    [Header("Attributes")]
    [SerializeField] private int baseNumEnemies = 8;
    [SerializeField] private float spawnRate = 0.5f;
    [SerializeField] private float timeUntilWave = 10f;
    [SerializeField] private float scalingFactor = 0.75f;
    [SerializeField] public static bool inBuildMode;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    public static int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesRemaining;
    private bool isSpawning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one Spawner found! Destroying duplicate.");
            Destroy(gameObject);
        }
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    // private void Start()
    // {
    //     StartWave();
    // }

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

        if (enemiesAlive == 0 && enemiesRemaining == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive --;
    }

    public void StartWave()
    {
        isSpawning = true;
        inBuildMode = false;
        enemiesRemaining = EnemiesPerWave(); 
    }

    private void EndWave()
    {
        isSpawning = false;
        LevelManager.main.IncreaseCurrency(currentWave * 10);
        timeSinceLastSpawn = 0f;
        currentWave++;
        inBuildMode = true;
        GameManager.instance.SetCurrentWave(currentWave);
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

    public void LoadWave(int wave)
    {
        currentWave = wave;
        GameManager.instance.SetCurrentWave(currentWave); // Update the current wave in GameManager
        isSpawning = false;
        inBuildMode = true;
    }

}
