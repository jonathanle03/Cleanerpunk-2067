using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class Enemy : MonoBehaviour
{
    public string enemyName;
    [SerializeField] public int health;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] TextMeshProUGUI enemyHealth;
    [SerializeField] public GameObject enemyButton;

    private void Start()
    {
        enemyHealth.text = "Health: " + health;
    }
    public void TakeDamage(float cardPower, int playerAttack)
    {
        int incomingDmg = Mathf.CeilToInt(cardPower * playerAttack * 100f / (100f + defense));
        health -= incomingDmg;
        enemyHealth.text = "Health: " + health;
        Debug.Log($"{enemyName} takes {incomingDmg} damage");
        
        if (health < 0)
        {
            Debug.Log($"{enemyName} is dead");
        }
    }
}
