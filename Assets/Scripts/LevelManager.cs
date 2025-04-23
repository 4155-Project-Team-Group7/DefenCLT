using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform start;
    public Transform[] path;
    public int currency;
    
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
        GameManager.instance.SetCurrency(currency);
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        GameManager.instance.SetCurrency(currency);
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            GameManager.instance.SetCurrency(currency); 
            return true;
        }

        else
        {
            Debug.Log("Not enough Currency");
            return false;
        }
    }

    public void LoadCurrency(int amount)
    {
        currency = amount;
        GameManager.instance.SetCurrency(currency);
    }

}