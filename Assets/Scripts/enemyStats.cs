using UnityEngine;

public class enemyStats : MonoBehaviour
{
    // This class is very similar to playerStats but simplified since the enemy won't do as much.
    // Basically just tracks the hp. 


    private int _enemyHealth;
    [SerializeField] int _maxEnemyHealth = 10;

    [SerializeField] int _enemyAttack = 1;

    private void Start()
    {
        _enemyHealth = _maxEnemyHealth; 
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
