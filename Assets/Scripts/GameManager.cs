using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Gameplay state
    public int currentWave { get; private set; } = 1;
    public int playerHealth { get; private set; } = 10;

    public Turret Turret {get; private set; } // Reference to the Turret script

    // public int Map = 2

    // Placed turrets tracking
    private List<TurretData> placedTurrets = new List<TurretData>();

    private void Awake()
    {
        if (instance = null){
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
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
        placedTurrets.Add(new TurretData
        {
            position = position,
            turretType = turretType,
            upgradeLevel = upgradeLevel
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


    


}