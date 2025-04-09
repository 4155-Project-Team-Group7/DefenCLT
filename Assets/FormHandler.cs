using UnityEngine;
using TMPro;

public class RegistrationHandler : MonoBehaviour
{
    // Public references to the UI InputFields
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField checkPasswordInput;

    // This method is called when the user presses the register button
    public void OnRegisterButtonClick()
    {
        // Retrieve text from each input field
        string email = emailInput.text;
        string password = passwordInput.text;
        string checkPassword = checkPasswordInput.text;

        // Output the input values to the console (or handle registration logic)
        Debug.Log("Username: " + email);
        Debug.Log("Email: " + password);
        Debug.Log("Password: " + checkPassword);

        // Continue with registration handling (e.g., send data to your back-end)
    }
}

