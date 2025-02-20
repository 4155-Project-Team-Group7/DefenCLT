using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathInd = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        target = LevelManager.main.path[pathInd];
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathInd++;
            if (pathInd == LevelManager.main.path.Length) 
            {
                Destroy(gameObject);
                return;
            } else
            {
                target = LevelManager.main.path[pathInd];
            }
        }
    }
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
}
