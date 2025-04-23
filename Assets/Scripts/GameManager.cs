using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay State")]
    public int currentWave { get; private set; } = 1;
    public int playerHealth { get; private set; } = 10;

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
            return;
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

        foreach (var turretData in savedTurrets)
        {
            Debug.Log($"Loading turret: {turretData.prefabName} at {turretData.position}");
            if (prefabLookup.TryGetValue(turretData.prefabName, out GameObject prefab))
            {
                GameObject turretGO = Instantiate(prefab, turretData.position, Quaternion.identity);

                // Optional: Apply upgrade level if your turret script supports it
                // Turret turretScript = turretGO.GetComponent<Turret>();
                // turretScript.SetUpgradeLevel(turretData.upgradeLevel);
            }
            else
            {
                Debug.LogWarning($"Prefab not found for turret: {turretData.prefabName}");
            }

            placedTurrets.Add(turretData);
        }
    }

    // Save the game
    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    // Load the game
    public void LoadGame()
    {
        var loadedData = SaveSystem.LoadGame();
        
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
        Debug.Log("New game started.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SaveGame();
            Debug.Log("Game saved.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            LoadGame();
            Debug.Log("Game loaded.");
        }
    }

}

[System.Serializable]
public struct TurretData
{
    public Vector2 position;
    public string prefabName;
    // public int upgradeLevel;
}


// [System.Serializable]
// public class SceneTurretData
// {
//     public TurretData[] turrets;
// }