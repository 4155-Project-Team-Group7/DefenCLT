using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRowUI : MonoBehaviour
{
    [Tooltip("Drag the Username Text component here")]
    public TextMeshProUGUI usernameText;

    [Tooltip("Drag the Wave Text component here")]
    public TextMeshProUGUI waveText;

    /// <summary>
    /// Call this to fill the row.
    /// </summary>
    public void Setup(string username, int wave)
    {
        usernameText.text = username;
        waveText.text    = wave.ToString();
    }
}
