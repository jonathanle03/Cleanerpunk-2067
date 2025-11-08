using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    [SerializeField] public CardData enemyCard;

    public string enemyName;
    public int health;
    public int enemyAttack = 5;
    [SerializeField] TextMeshProUGUI enemyHealth;
    [SerializeField] public GameObject enemyButton;

    public bool isDead;

    private void Start()
    {
        enemyHealth.text = "Health: " + health;
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        enemyHealth.text = "Health: " + health;
        //Debug.Log($"{enemyName} takes {amount} damage");
        
        if (health < 0)
        {
            //Debug.Log($"{enemyName} is dead");
        }
    }
}
