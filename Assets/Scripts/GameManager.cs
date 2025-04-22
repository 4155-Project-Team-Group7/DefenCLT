using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay State")]
    public int currentWave { get; private set; } = 1;
    public int playerHealth { get; private set; } = 10;

    [Header("Turret Tracking")]
    [SerializeField] private List<GameObject> turretPrefabs; // Assign in Inspector
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
    }

    public void SetCurrentWave(int wave)
    {
        currentWave = wave;
    }

    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
    }

    public void RegisterTurret(Vector2 position, string turretType, int upgradeLevel)
    {
        GameObject prefab = turretPrefabs.Find(t => t.name == turretType);

        placedTurrets.Add(new TurretData
        {
            position = position,
            turretType = turretType,
            upgradeLevel = upgradeLevel,
            turretPrefab = prefab
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
            GameObject prefab = turretPrefabs.Find(t => t.name == turretData.turretType);
            if (prefab != null)
            {
                GameObject turretGO = Instantiate(prefab, turretData.position, Quaternion.identity);
                Turret turretScript = turretGO.GetComponent<Turret>();
                turretScript.SetUpgradeLevel(turretData.upgradeLevel);
            }

            placedTurrets.Add(turretData);
        }
    }
}

[System.Serializable]
public struct TurretData
{
    public Vector2 position;
    public string turretType;
    public int upgradeLevel;
    public GameObject turretPrefab;
}
