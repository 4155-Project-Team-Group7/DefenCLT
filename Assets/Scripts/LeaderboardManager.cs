using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class LeaderboardEntry
{
    public string Uname;
    public int Wv;
    public LeaderboardEntry(string username, int wave)
    {
        Uname = username;
        Wv = wave;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI Setup")]
    public Transform rowParent; 
    public GameObject rowPrefab; 


    void Awake()
    {
        // Debug.Log("[LB] Start(): populating top 5");
        // PopulateTop5();
    }
    [ContextMenu("Populate Top 5")]
    public void PopulateTop5()
    {
        var db = FirebaseFirestore.DefaultInstance;
        Debug.Log("[LB] PopulateTop5()");
        db.Collection("Leaderboard")
          .OrderByDescending("wave")    // ← updated here
          .Limit(5)
          .GetSnapshotAsync()
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsFaulted || task.IsCanceled)
              {
                  Debug.LogError($"[Firestore] Failed to fetch leaderboard: {task.Exception}");
                  return;
              }
              Debug.Log($"[Firestore] Leaderboard fetched, {task.Result.Count} documents found.");  
              var docs = task.Result.Documents;
              List<LeaderboardEntry> top5 = new List<LeaderboardEntry>();
              foreach (var doc in docs)
              {
                  string name = doc.ContainsField("username")
                                ? doc.GetValue<string>("username")
                                : "Unknown";
                  int wave = doc.ContainsField("wave")
                             ? doc.GetValue<int>("wave")
                             : 0;
                    Debug.Log($"[LB] {name} - {wave}");
                  top5.Add(new LeaderboardEntry(name, wave));
              }
            Debug.Log(top5[0].Uname + " - " + top5[0].Wv + " - " + top5[1].Uname + " - " + top5[1].Wv);
              UpdateUI(top5);
          });
    }

    // void UpdateUI(List<LeaderboardEntry> entries)
    // {
    //     // Clear old rows
    //     foreach (Transform child in rowParent)
    //         Destroy(child.gameObject);

    //     // Instantiate new rows
    //     foreach (var e in entries)
    //     {
    //         Debug.Log($"[LB] {e.Uname} - {e.Wv}");
    //         var row = Instantiate(rowPrefab, rowParent);
    //         var texts = row.GetComponentsInChildren<Text>();
    //         texts[0].text = e.Uname;
    //         texts[1].text = e.Wv.ToString();
    //     }
    // }

    void UpdateUI(List<LeaderboardEntry> entries)
{
    // Clear old rows
    foreach (Transform child in rowParent)
        Destroy(child.gameObject);

    // Instantiate new rows
    foreach (var e in entries)
    {
        // 1) Spawn the prefab under your layout group
        var rowGO = Instantiate(rowPrefab, rowParent, false);

        // 2) Grab the LeaderboardRowUI script and drive it
        var rowUI = rowGO.GetComponent<LeaderboardRowUI>();
        if (rowUI == null)
        {
            Debug.LogError("[LB] Row prefab is missing a LeaderboardRowUI component!");
            continue;
        }

        // 3) Fill in the data
        rowUI.Setup(e.Uname, e.Wv);

        // (optional) log again to confirm
        Debug.Log($"[LB] Placed row → {e.Uname} : {e.Wv}");
    }
}
}
