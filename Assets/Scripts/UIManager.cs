using UnityEngine;
using Firebase;
using Firebase.Auth;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject mainMenuUI;

    private FirebaseAuth auth;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;
    }

    private void Start()
    {
        // Check if user is already logged in
        if (auth.CurrentUser != null)
        {
            Debug.Log("User already logged in: " + auth.CurrentUser.Email);
            MainMenuScreen(); // Skip login and go to main menu
        }
        else
        {
            Debug.Log("No user logged in.");
            LoginScreen(); // Show login screen
        }
    }

    // Functions to change the login screen UI
    public void LoginScreen()
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        mainMenuUI.SetActive(false);
    }

    public void RegisterScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void MainMenuScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
