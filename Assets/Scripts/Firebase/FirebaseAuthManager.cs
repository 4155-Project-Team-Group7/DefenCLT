using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour {
    private FirebaseAuth auth;

    void Start() {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void RegisterUser(string email, string password) {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCompleted && !task.IsFaulted) {
                FirebaseUser newUser = task.Result;
                Debug.Log("User Created: " + newUser.UserId);
            } else {
                Debug.LogError("Registration failed: " + task.Exception);
            }
        });
    }

    public void LoginUser(string email, string password) {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCompleted && !task.IsFaulted) {
                FirebaseUser user = task.Result;
                Debug.Log("User Logged In: " + user.UserId);
            } else {
                Debug.LogError("Login failed: " + task.Exception);
            }
        });
    }
}