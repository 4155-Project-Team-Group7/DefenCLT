using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    
    [Header("Attributes")]
    [SerializeField] private float targetRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; //fire rate
 
    private Transform target;
    private float timeUntilFire;



    //checking if there is an enemy and roataing the turret to it
    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetInRange())
        {
            target = null;
        }

        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                shoot();
                timeUntilFire = 0f;
            }
        }
    }


    private void shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);

    }


    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetRange, (Vector2) transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetInRange()
    {
        return  Vector2.Distance(target.position, transform.position) <= targetRange; 
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }




}

