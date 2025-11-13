using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Enemy : Entity
{
    [Header("Enemy Specific Info")]
    public CardData enemyCard;
    [SerializeField] public GameObject enemyButton;

    public bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
        RandomCard();
    }

    public override void EntityDead()
    {
        isDead = true;
        enemyButton.SetActive(false);
    }

    public void RandomCard()
    {
        enemyCard = deck[Random.Range(0, deck.Count)];
    }
}
