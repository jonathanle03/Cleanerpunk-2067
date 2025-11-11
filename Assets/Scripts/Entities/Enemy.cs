using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int health;
    [SerializeField] TextMeshProUGUI enemyHealth;
    [SerializeField] public GameObject enemyButton;

    private void Start()
    {
        enemyHealth.text = "Health: " + health;
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        enemyHealth.text = "Health: " + health;
        Debug.Log($"{enemyName} takes {amount} damage");
        
        if (health < 0)
        {
            Debug.Log($"{enemyName} is dead");
        }
    }
}
