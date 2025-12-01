using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections; 

public class Enemy : Entity
{
    [Header("Enemy Specific Info")]
    [SerializeField] public EnemyData enemyData;
    public Sprite art;
    public CardData enemyCard;
    [SerializeField] public GameObject enemyButton;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float _timing; 

    public bool isDead;

    private void Start()
    {
        
    }

    private void Update()
    {
        healthText.text = "" + currentHealth;
        healthBar.GetComponent<Transform>().localScale = new Vector3((float)currentHealth / maxHealth * healthBarMaxWidth, healthBar.GetComponent<Transform>().localScale.y, healthBar.GetComponent<Transform>().localScale.z);
        healthBar.GetComponent<Transform>().localPosition = new Vector3(healthBarStartPos.x - (healthBarMaxWidth - healthBar.GetComponent<Transform>().localScale.x) / 2 * healthBar.GetComponent<RectTransform>().rect.width, healthBarStartPos.y, healthBarStartPos.z);
    }

    public override void EntityDead()
    {
        isDead = true;
        enemyButton.SetActive(false);
    }

    public void InitializeEnemy(EnemyData data)
    {
        enemyData = data;
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
        healthBarMaxWidth = (int)healthBar.GetComponent<Transform>().localScale.x;
        healthBarStartPos = healthBar.GetComponent<Transform>().localPosition;
        healthText.text = "Health: " + currentHealth;
        RandomCard();
    }

    public void RandomCard()
    {
        enemyCard = deck[Random.Range(0, deck.Count)];
    }


    override public void TakeDamage(float cardPower, int incomingAttack)
    {
        float incomingDmg = cardPower * incomingAttack * 100f / (100f + defense);
        foreach (StatusEffect effect in statusEffects)
        {
            incomingDmg = effect.ModifyIncomingDamage(incomingDmg);
        }

        currentHealth -= Mathf.CeilToInt(incomingDmg);
        healthText.text = "Health: " + currentHealth;
        Debug.Log($"{entityName} takes {incomingDmg} damage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log($"{entityName} is dead");
            EntityDead();
        }

        StartCoroutine(turnRed()); 
    }


    IEnumerator turnRed()
    {
        sprite.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(_timing);
        sprite.color = Color.white; 
    }

}
