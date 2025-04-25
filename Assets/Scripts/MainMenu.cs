using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class MainMenu : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
        {
            SceneManager.LoadSceneAsync(1);
        }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ExitToMenu()
    {
        SaveTestData();
        SceneManager.LoadSceneAsync(0);
    }

    void SaveTestData()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var user = auth.CurrentUser;
        if (user == null)
        {
            Debug.LogError("[Firestore] no user signed in, skipping write.");
            return;
        }

        var db = FirebaseFirestore.DefaultInstance;

        // build a small test payload
        var testDoc = new Dictionary<string, object>
        {
            { "username", user.DisplayName ?? user.Email },
            { "testValue", Random.Range(0, 1000) },
            { "createdAt", Timestamp.GetCurrentTimestamp() }
        };

        // write under a "testData" collection using auto‐ID
        db.Collection("testData")
          .AddAsync(testDoc)
          .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
                Debug.LogError($"[Firestore] failed to write test data: {task.Exception}");
            else
                Debug.Log("[Firestore] test data written, ID = " + task.Result.Id);
        });
    }
    
}
