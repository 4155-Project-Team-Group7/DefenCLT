using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }
    
    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null)
        {
            Debug.Log("You already have a turret here!");
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You can't afford this");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        GameManager.instance.RegisterTurret(transform.position, towerToBuild.prefab);

    }

    public void ClearTurret()
    {
        if (tower != null)
        {
            Destroy(tower);
            tower = null;
        }
    }

    public void LoadTurret(GameObject turretPrefab)
    {
        ClearTurret(); // Just in case there's already one
        tower = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        GameManager.instance.RegisterTurret(transform.position, turretPrefab);
    }
}