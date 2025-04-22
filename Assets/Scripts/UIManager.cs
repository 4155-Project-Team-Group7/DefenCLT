using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // UI Screens
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject mainMenuUI;

    // Gameplay UI Elements
    public Button startButton;
    public Button stopButton;
    public Text scoreText;

    // Gameplay state
    private int currentScore = 0;
    private bool isGameRunning = false;
    private Coroutine waveCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("UIManager instance already exists. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Assign button listeners
        if (startButton != null)
            startButton.onClick.AddListener(StartGame);

        if (stopButton != null)
            stopButton.onClick.AddListener(StopGame);

        // Start at login screen
        LoginScreen();
        UpdateScoreText();
        ShowButtons(false); // Hide start/stop until main menu
    }

    // UI Navigation
    public void LoginScreen()
    {
        loginUI?.SetActive(true);
        registerUI?.SetActive(false);
        mainMenuUI?.SetActive(false);
        ShowButtons(false);
    }

    public void RegisterScreen()
    {
        loginUI?.SetActive(false);
        registerUI?.SetActive(true);
        mainMenuUI?.SetActive(false);
        ShowButtons(false);
    }

    public void MainMenuScreen()
    {
        loginUI?.SetActive(false);
        registerUI?.SetActive(false);
        mainMenuUI?.SetActive(true);
        ShowButtons(true);
    }

    // Gameplay Controls
    public void StartGame()
    {
        if (isGameRunning)
        {
            Debug.Log("Game already running.");
            return;
        }

        isGameRunning = true;
        ResetScore();
        Debug.Log("Game Started!");
        waveCoroutine = StartCoroutine(SimulateWaves());
    }

    public void StopGame()
    {
        if (!isGameRunning)
        {
            Debug.Log("Game is not running.");
            return;
        }

        isGameRunning = false;
        if (waveCoroutine != null)
            StopCoroutine(waveCoroutine);

        Debug.Log("Game Stopped!");
    }

    // Simulated Wave Coroutine (Gameplay Logic)
    private IEnumerator SimulateWaves()
    {
        int wave = 1;
        while (isGameRunning)
        {
            Debug.Log($"Wave {wave} started!");

            int enemies = Random.Range(3, 6); // simulate 3-5 enemies per wave
            for (int i = 0; i < enemies; i++)
            {
                yield return new WaitForSeconds(1f); // simulate delay per enemy
                AddScore(10); // score for defeating an enemy
                Debug.Log($"Enemy {i + 1} defeated.");
            }

            Debug.Log($"Wave {wave} completed!");
            wave++;

            yield return new WaitForSeconds(3f); // delay between waves
        }
    }

    // Score Management
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
        else
        {
            Debug.LogWarning("Score Text not assigned.");
        }
    }

    private void ShowButtons(bool show)
    {
        if (startButton != null)
            startButton.gameObject.SetActive(show);
        if (stopButton != null)
            stopButton.gameObject.SetActive(show);
    }
}
