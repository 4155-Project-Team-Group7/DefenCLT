using Firebase.Database;
using System.Collections.Generic;

public void SaveGameData(string userId, int roundNumber, List<Tower> towers) {
    DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    
    Dictionary<string, object> gameData = new Dictionary<string, object>();
    gameData["round_number"] = roundNumber;
    
    Dictionary<string, object> towerData = new Dictionary<string, object>();
    for (int i = 0; i < towers.Count; i++) {
        towerData[$"tower_{i+1}"] = new Dictionary<string, object> {
            { "type", towers[i].type },
            { "upgrade", towers[i].upgrade },
            { "x", towers[i].x },
            { "y", towers[i].y }
        };
    }

    gameData["towers"] = towerData;

    dbRef.Child("users").Child(userId).Child("GameState").SetRawJsonValueAsync(JsonUtility.ToJson(gameData));
}