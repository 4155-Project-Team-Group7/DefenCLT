using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay State")]
    public int currentWave { get; private set; } = 1;
    public int playerHealth { get; private set; } = 10;

    public int currency { get; private set; } = 0;

    public bool startNewGame = false; // Set this to true to start a fresh game
    public bool loadGameOnStart = false; // Set this to true to load a game on start

    [Header("Turret Prefabs")]
    [SerializeField] private List<GameObject> turretPrefabs; 
    private Dictionary<string, GameObject> prefabLookup;

    private List<TurretData> placedTurrets = new List<TurretData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Build prefab lookup dictionary
        turretPrefabs = Resources.LoadAll<GameObject>("Turrets").ToList();
        prefabLookup = turretPrefabs.ToDictionary(p => p.name, p => p);
    }


    public void SetCurrentWave(int wave)
    {
        currentWave = wave;
    }

    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
    }

    public void SetCurrency(int amount)
    {
        currency = amount;
    }

    public void RegisterTurret(Vector2 position, GameObject turretPrefab /*, int upgradeLevel*/ )
    {
        placedTurrets.Add(new TurretData
        {
            position = position,
            prefabName = turretPrefab.name,
            // upgradeLevel = upgradeLevel // Uncomment if you have an upgrade system
        });
    }

    public List<TurretData> GetPlacedTurrets()
    {
        return placedTurrets;
    }

    public void ClearTurrets()
    {
        placedTurrets.Clear();
    }

    public void LoadTurrets(TurretData[] savedTurrets)
    {
        ClearTurrets();
        Plot[] allPlots = FindObjectsByType<Plot>(FindObjectsSortMode.None);

        foreach (Plot plot in allPlots)
        {
            plot.ClearTurret();
        }

        foreach (var turretData in savedTurrets)
        {
            if (prefabLookup.TryGetValue(turretData.prefabName, out GameObject prefab))
            {
                Plot plot = allPlots.FirstOrDefault(p => Vector2.Distance(p.transform.position, turretData.position) < 0.1f);

                if (plot != null)
                {
                    plot.LoadTurret(prefab);
                }
                else
                {
                    Debug.LogWarning($"No plot found at position {turretData.position}");
                }
            }
            else
            {
                Debug.LogWarning($"Prefab not found: {turretData.prefabName}");
            }

            // placedTurrets.Add(turretData);
        }
    }

    // Save the game
    public void SaveGame()
    {
        SaveSystem.SaveGame();
        startNewGame = false; // Reset to false after saving
        Debug.Log("Game saved.");
    }

    // Load the game
    public void LoadGame()
    {
        var loadedData = SaveSystem.LoadGame();
        if (loadedData.Equals(default(SaveSystem.SceneData)))
        {
            Debug.LogWarning("No saved data found. Skipping LoadGame.");
            startNewGame = true; // Set to true to start a new game
            return;
        }
        // Loading Scene
        Debug.Log($"Loaded scene: {loadedData.sceneName}");

        // Load Current Wave
        Spawner.instance.LoadWave(loadedData.currentWave);

        // Load Currency
        LevelManager.main.LoadCurrency(loadedData.currency);
        
        // Convert SaveSystem.TurretSaveData back into TurretData
        TurretData[] turretData = loadedData.turrets.Select(t => new TurretData
        {
            position = t.position,
            prefabName = t.prefabName
        }).ToArray();

        LoadTurrets(turretData);
    }

    // Optional: Save on New Game
    public void StartNewGame()
    {
        SaveSystem.ClearSave();
        ClearTurrets();
        currentWave = 1;
        playerHealth = 10;
        startNewGame = false;
        Debug.Log("New game started.");
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Alpha9))
    //     {
    //         SaveGame();
    //         Debug.Log("Game saved.");
    //     }

    //     if (Input.GetKeyDown(KeyCode.Alpha0))
    //     {
    //         LoadGame();
    //         Debug.Log("Game loaded.");
    //     }
    // }

}

[System.Serializable]
public struct TurretData
{
    public Vector2 position;
    public string prefabName;
    // public int upgradeLevel;
}

