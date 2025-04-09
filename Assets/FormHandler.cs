using UnityEngine;
using TMPro;
using Firebase.Auth; // Ensure you have the Firebase Unity SDK installed
using Firebase.Extensions; // For ContinueWith extension method


public class RegistrationHandler : MonoBehaviour
{
    // Public references to the UI InputFields
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField checkPasswordInput;

    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    // Method to create a new user
    public void CreateUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string checkPassword = checkPasswordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
        if (task.IsCanceled) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            return;
        }

        // Firebase user has been created.
        Firebase.Auth.AuthResult result = task.Result;
        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);
        });
    }

    // This method is called when the user presses the register button

}

