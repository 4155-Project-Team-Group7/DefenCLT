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
}

