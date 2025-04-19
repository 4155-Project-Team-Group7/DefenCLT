using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    [System.Serializable]
    public struct SaveData
    {
        public TurretSaveData[] turrets;
    }

    public static void SaveGame(SceneTurretData sceneData)
    {
        SaveData saveData = new SaveData();
        saveData.turrets = sceneData.turrets;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(GetSavePath(), json);
    }

    public static SceneTurretData LoadGame()
    {
        if (!File.Exists(GetSavePath()))
        {
            Debug.LogWarning("No save file found!");
            return new SceneTurretData();
        }

        string json = File.ReadAllText(GetSavePath());
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        SceneTurretData result = new SceneTurretData();
        result.turrets = loadedData.turrets;
        return result;
    }

    private static string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "savegame.json");
    }
}