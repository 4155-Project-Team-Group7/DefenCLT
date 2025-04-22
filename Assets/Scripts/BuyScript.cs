using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyScript : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] TextMeshProUGUI currencyUI;

    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
    }

    public void SetSelectedTower()
    {
        
    }
}
