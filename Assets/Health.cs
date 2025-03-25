using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int hp = 2;
    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if(hp <= 0)
        {
            Spawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
