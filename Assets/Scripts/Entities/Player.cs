using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;
  
    [SerializeField] PlayerStats stats;
    [SerializeField] Enemy enemy;
    [Header("Cards")]
    [SerializeField] List<CardData> deck = new List<CardData>();
    [SerializeField] CardData[] hand = new CardData[6];
    [SerializeField] List<CardData> discards = new List<CardData>();
    private int selectedCard;
    private int usedCards;

    [SerializeField] public GameObject[] enemyList = new GameObject[3];
    [Header("Stats")]
    [SerializeField] public int health = 100;
    [SerializeField] public int attack = 100;
    [SerializeField] public int defense = 100;

    public const int maxMana = 9;
    public int currentMana;
    
    void Start()
    {
        currentMana = maxMana;

        deck = stats.getDeck();



        for(int i = 0; i < 6;  i++)
        {
            Debug.Log("index: " + i);
            hand[i] = deck[0];
            deck.RemoveAt(0);
            Debug.Log("filling index: " + i);
        }

    }
    private void Update()
    {
        energyText.text = "Energy: " + currentMana;
    }

    public void useCard(int pick)
    {
        if(currentMana >= hand[pick].cost)
        {
            selectedCard = pick;
            currentMana -= hand[pick].cost;
            if (hand[pick].target == CardTarget.Enemy)
            {
                enemy.enemyButton.SetActive(true);
            }
        }
    }

    public void enemySelect(int pick)
    {
        Debug.Log("card selected: " + selectedCard + " enemy selected: " + pick);
        hand[selectedCard].doEffect(this, enemyList[pick].GetComponent<Enemy>());
        enemy.enemyButton.SetActive(false);
        discards.Add(hand[selectedCard]);

        hand[selectedCard] = null;
        usedCards++;

    }

    public void takeDamage(float cardPower, int enemyAttack)
    {
        int incomingDmg = Mathf.CeilToInt(cardPower * enemyAttack * 100f / (100f + defense));
        health -= incomingDmg;
        Debug.Log($"Player takes {incomingDmg} damage");
        
        if (health < 0)
        {
            Debug.Log($"Player is dead");
        }
    }

    void drawCard()
    {
        int cardDrawn = Random.Range(0, deck.Count - 1);
        for(int i = 0; i < hand.Length; i++)
        {
            if (hand[i] == null)
            {
                hand[i] = deck[cardDrawn];
                break;
            }
        }
        deck.RemoveAt(cardDrawn);
    }

    public void fillHand()
    {
        for(int i = 0; i < usedCards; i++)
        {
            drawCard();
        }
        usedCards = 0;
        currentMana = maxMana;
    }
}
