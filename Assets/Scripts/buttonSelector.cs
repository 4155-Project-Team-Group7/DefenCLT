using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    public GameObject[] buttons; // Assign your 4 buttons here in Inspector

    public void SelectButton(int index)
    {
        if (index >= 0 && index < buttons.Length)
        {
            EventSystem.current.SetSelectedGameObject(buttons[index]);
        }
    }
}
