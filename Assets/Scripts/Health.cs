using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{    
    [Header("Attributes")]
    [SerializeField] private int hitpoints = 2;
    
    public void TakeDamge(int dmg)
    {
        hitpoints -= dmg;

        if (hitpoints <= 0)
        {
            Spawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}