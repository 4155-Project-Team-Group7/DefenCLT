using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    [System.Serializable]
    public struct TurretSaveData
    {
        public Vector2 position;
        public string prefabName;
    }

    [System.Serializable]
    public struct SceneTurretData
    {
        public string sceneName;  // Store the scene name here
        public TurretSaveData[] turrets;
    }

    // Save the game state to a file, including the scene name
    public static void SaveGame()
    {
        // Collect the turret data from GameManager
        List<TurretSaveData> turretSaveList = new List<TurretSaveData>();

        foreach (var turret in GameManager.instance.GetPlacedTurrets())
        {
            turretSaveList.Add(new TurretSaveData
            {
                position = turret.position,
                prefabName = turret.prefabName
            });
        }

        // Get the current scene name
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Prepare the save data
        SceneTurretData sceneData = new SceneTurretData
        {
            sceneName = currentScene,
            turrets = turretSaveList.ToArray()
        };

        // Serialize the data
        string json = JsonUtility.ToJson(sceneData, true);

        // Write to file
        File.WriteAllText(GetSavePath(), json);
        Debug.Log("Game Saved!");
    }

    // Load the game state from a file
    public static SceneTurretData LoadGame()
    {
        if (!File.Exists(GetSavePath()))
        {
            Debug.LogWarning("No save file found!");
            return new SceneTurretData();  // Return an empty data structure
        }

        string json = File.ReadAllText(GetSavePath());
        SceneTurretData loadedData = JsonUtility.FromJson<SceneTurretData>(json);

        // Check if the save file is from the current scene
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (loadedData.sceneName != currentScene)
        {
            Debug.LogWarning("Saved data does not match the current scene.");
            return new SceneTurretData();  // Return empty data if the scene doesn't match
        }

        return loadedData;
    }

    private static string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    // Optional: Clear the save file
    public static void ClearSave()
    {
        if (File.Exists(GetSavePath()))
        {
            File.Delete(GetSavePath());
            Debug.Log("Save file cleared.");
        }
        else
        {
            Debug.LogWarning("No save file to clear.");
        }
    }
}
