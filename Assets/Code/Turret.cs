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

    // List and Dictionary for saving and loading turrets
    List<TurretSaveData> turretList = GetPlacedTurrets();
    private Dictionary<GameObject, GameObject> turretPrefabs = new Dictionary<GameObject, GameObject>();


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


// Saving and Loading mechanisms

    public List<TurretSaveData> GetPlacedTurrets()
    {
        foreach (var turret in FindObjectsOfType<Turret>())
        {
            TurretSaveData turretData = new TurretSaveData
            {
                position = turret.transform.position,
                turretType = turret.name, // Assuming the name of the GameObject is the type of turret
                upgradeLevel = 1 // Placeholder for upgrade level, implement your own logic here
            };
            turretList.Add(turretData);
            turretPrefabs[turret.gameObject] = turret.TurretPrefab; // Store the turret prefab reference
        }
        return turretSaveData;
    }
    public void Save(ref SceneTurretData data)
    {
        List<TurretSaveData> turretSaveData = GetPlacedTurrets();
        for (int i = turretList.Count - 1; i >= 0; i--)
        {
            if (turretList[i] != null)
            {
                GameObject turret = turretList[i];
                TurretSaveData turretData = new TurretSaveData
                {
                    position = turret.transform.position,
                    turretType = turret.name,
                    upgradeLevel = 1 // Placeholder for upgrade level, implement your own logic here
                };
                turretSaveData.Add(turretData);
            }
            else
            {
                turretList.RemoveAt(i); // Remove null entries
            }
        }
        data.Turrets = turretSaveData.ToArray(); // Convert to array for saving
    }

    public void LoadGame()
    {
        SceneTurretData data = SaveSystem.LoadGame();

        foreach(var turret in turretList)
        {
            if (turret != null)
            {
                Destroy(turret.gameObject); // Destroy existing turrets
            }
        }
        turretList.Clear();
        turretPrefabs.Clear();

        foreach (var turretData in data.turrets)
        {
            if (turretData.turretPrefab != null)
            {
                GameObject turret = Instantiate(turretData.turretPrefab, turretData.position, Quaternion.identity);
                turret.upgradeLevel = turretData.upgradeLevel; // Set the upgrade level
                turret.turretType = turretData.turretType; // Set the name of the turret
                turretList.Add(turret); // Add to the list of turrets
                turretPrefabs[turret] = turretData.turretPrefab;
            }
        }
    }


}

[System.Serializable]
public struct SceneTurretData
{
    public TurretSaveData[] Turrets;
}

[System.Serializable]
public struct TurretSaveData
{
    public Vector2 position;
    public string turretType;
    public int upgradeLevel;
    public GameObject turretPrefab;
}



