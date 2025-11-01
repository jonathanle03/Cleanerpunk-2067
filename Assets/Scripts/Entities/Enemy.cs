using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int health;
    
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{enemyName} takes {amount} damage");
        
        if (health < 0)
        {
            Debug.Log($"{enemyName} is dead");
        }
    }
}
