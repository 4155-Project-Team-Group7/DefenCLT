using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class MainMenu : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public async void PlayGame()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(1);

        // Wait until the scene is fully loaded
        while (!loadOp.isDone)
            await Task.Yield();

        // // Extra: Wait for LevelManager and Spawner to initialize
        await WaitForSceneSetup();
        // start a new game
        if (GameManager.instance.startNewGame)
        {
            GameManager.instance.StartNewGame();
        }
        else
        {
            GameManager.instance.LoadGame();
        }
        

        // GameManager.instance.LoadGame();
        // GameManager.instance.StartNewGame();
    }

    private async Task WaitForSceneSetup()
    {
        // Wait until LevelManager and Spawner are fully initialized
        while (LevelManager.main == null || Spawner.instance == null || FindObjectsByType<Plot>(FindObjectsSortMode.None).Length == 0)
        {
            await Task.Yield();
        }
    }

    public void QuitGame()
    {
        Application.Quit();

    }

    public void ExitToMenu()
    {
        SaveTestData();
        SceneManager.LoadSceneAsync(0);
        GameManager.instance.SaveGame(); // Save the game before exiting to menu
        Debug.Log("Exiting to menu...");
    }

    void SaveTestData()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var user = auth.CurrentUser;
        // var level = LevelManager.main;
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
            { "Score", LevelManager.main.currency },
            { "wave", Spawner.currentWave },
            { "createdAt", Timestamp.GetCurrentTimestamp() }
        };

        // write under a "testData" collection using auto‐ID
        db.Collection("Leaderboard")
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
