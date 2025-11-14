using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Enemy : Entity
{
    [Header("Enemy Specific Info")]
    [SerializeField] public EnemyData enemyData;
    public Sprite art;
    public CardData enemyCard;
    [SerializeField] public GameObject enemyButton;

    public bool isDead;

    private void Start()
    {
        entityName = enemyData.enemyName;
        art = enemyData.art;
        maxHealth = enemyData.maxHealth;
        attack = enemyData.attack;
        defense = enemyData.defense;
        isBoss = enemyData.isBoss;
        deck = new List<CardData>(enemyData.cards);

        SpriteRenderer thisrender = GetComponent<SpriteRenderer>();
        thisrender.sprite = art; 

        currentHealth = maxHealth;
        healthText.text = "Health: " + currentHealth;
        RandomCard();
    }

    private void Update()
    {
        healthText.text = "Health: " + currentHealth;
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
