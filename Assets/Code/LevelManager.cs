using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform start;
    public Transform[] path;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        main = this;
    }
}