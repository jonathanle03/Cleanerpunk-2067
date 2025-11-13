using UnityEngine;
using TMPro;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour
{
    [Header("Basic Info")]
    public string entityName;
    public int maxHealth;
    public int currentHealth;
    public int attack;
    public int defense;

    [Header("Additional Info")]
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public List<CardData> deck = new List<CardData>();

    public void TakeDamage(float cardPower, int incomingAttack)
    {
        int incomingDmg = Mathf.CeilToInt(cardPower * incomingAttack * 100f / (100f + defense));
        currentHealth -= incomingDmg;
        healthText.text = "Health: " + currentHealth;
        Debug.Log($"{entityName} takes {incomingDmg} damage");

        if (currentHealth <= 0)
        {
            Debug.Log($"{entityName} is dead");
            EntityDead();
        }
    }

    public abstract void EntityDead();

}
