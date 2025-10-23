using UnityEngine;

public class enemyStats : MonoBehaviour
{
    
    [SerializeField] int _enemyHealth = 10;
    [SerializeField] int _maxEnemyHealth = 10;

    [SerializeField] int _enemyAttack = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getHealth()
    {
        return _enemyHealth;
    }

    public void setHealth(int newHealth)
    {
        _enemyHealth = newHealth;
    }

    public void damageHealth(int damage)
    {
        _enemyHealth -= damage;
    }

    public int getMaxHealth()
    {
        return _maxEnemyHealth;
    }

    public int getAttack()
    {
        return _enemyAttack;
    }
}
