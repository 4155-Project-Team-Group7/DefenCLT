using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{    
    [Header("Attributes")]
    [SerializeField] private int hitpoints = 2;
    [SerializeField] private int points = 50;

    private bool isDestroyed = false;
    
    public void TakeDamge(int dmg)
    {
        hitpoints -= dmg;

        if (hitpoints <= 0 && !isDestroyed)
        {
            Spawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(points);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}