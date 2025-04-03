using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentRound = 1;
    public int totalRounds = 10;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize game state
        StartGame();
    }

    public void StartGame()
    {
        currentRound = 1;
        isGameOver = false;
        // LoadRound(currentRound); Load the first round
    }

    public void LoadGameData(string userId)
    {
        // Load game data from Firebase
    }

    public void SaveGameData(string userId)
    {
        // Save game data to Firebase
    }

    public void NextRound()
    {
        if (currentRound < totalRounds)
        {
            currentRound++;
            // LoadRound(currentRound); Load the next round
        }
        else
        {
            EndGame();
        }
    }
    public void EndGame()
    {
        isGameOver = true;
        // Show game over screen
    }
}