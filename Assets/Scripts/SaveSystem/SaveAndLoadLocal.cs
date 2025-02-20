using System;
using System.IO;
using UnityEngine;

public class SaveAndLoadLocal : MonoBehaviour
{
    void Start()
    {
        Save();
        // Load();
    }

    void Save()
    {
        SaveDataModel saveData = new SaveDataModel
        {
            playerName = "Player",
            playerLevel = 15,
            playerPosition = new Vector3(0, 50, 20)
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        Debug.Log("Save Data: " + Application.persistentDataPath + "/save.json");
    }

    void Load()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
        SaveDataModel saveData = JsonUtility.FromJson<SaveDataModel>(json);

        Debug.Log("Player Name: " + saveData.playerName);
        Debug.Log("Player Level: " + saveData.playerLevel);
        Debug.Log("Player Position: " + saveData.playerPosition);
    }

}

[Serializable]
public class SaveDataModel
{
    public string playerName;
    public int playerLevel;
    public Vector3 playerPosition;
}