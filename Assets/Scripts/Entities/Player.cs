using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] GameObject[] cardButtons = new GameObject[6];
    [SerializeField] TextMeshProUGUI[] buttonText = new TextMeshProUGUI[6];
    [SerializeField] GameObject passButton;
    private int chosenCard;

    [Header("Class References")]
    [SerializeField] PlayerStats stats;
    [SerializeField] Enemy enemy;
    [SerializeField] EnemyManager enemyManager;

    public bool playerTurn = true;


    [Header("Cards")]
    [SerializeField] List<CardData> deck = new List<CardData>();
    [SerializeField] CardData[] hand = new CardData[6];
    [SerializeField] List<CardData> discards = new List<CardData>();
    private int selectedCard;
    private int usedCards;

    [Header("Stats")]
    [SerializeField] public int maxhealth = 100;
    public int currentHealth;
    [SerializeField] TextMeshProUGUI playerHealthText;

    [SerializeField] public int health = 100;
    [SerializeField] public int attack = 100;
    [SerializeField] public int defense = 100;

    public const int maxMana = 9;
    public int currentMana;

    void Start()
    {
        currentHealth = maxhealth;
        playerHealthText.text = "Health: " + currentHealth;
        currentMana = maxMana;

        deck = stats.getDeck();

        for (int i = 0; i < 6; i++)
        {
            //Debug.Log("index: " + i);
            hand[i] = deck[0];
            deck.RemoveAt(0);
            //Debug.Log("filling index: " + i);
        }

        nameCards();

    }
    private void Update()
    {
        energyText.text = "Energy: " + currentMana;
    }

    public void useCard(int pick)
    {
        if (currentMana >= hand[pick].cost)
        {
            selectedCard = pick;
            currentMana -= hand[pick].cost;
            if (hand[pick].target == CardTarget.Enemy)
            {
                for (int i = 0; i < 3; i++)
                {
                    enemyManager.enemyButtons[i].SetActive(true);
                }

                chosenCard = pick;
            }
        }
    }

    public void enemySelect(int pick)
    {
        //Debug.Log("card selected: " + selectedCard + " enemy selected: " + pick);
        hand[selectedCard].doEffect(this, enemyManager.enemies[pick].GetComponent<Enemy>());
        for (int i = 0; i < 3; i++)
        {
            enemyManager.enemyButtons[i].SetActive(false);
        }
        discards.Add(hand[selectedCard]);

        cardButtons[chosenCard].SetActive(false);
        hand[selectedCard] = null;
        usedCards++;

    }

    public void takeDamage(float cardPower, int enemyAttack)
    {
        int incomingDmg = Mathf.CeilToInt(cardPower * enemyAttack * 100f / (100f + defense));
        currentHealth -= incomingDmg;
        playerHealthText.text = "Health: " + currentHealth;
        Debug.Log($"Player takes {incomingDmg} damage");

        if (currentHealth < 0)
        {
            Debug.Log($"Player is dead");
            playerDead();
        }
    }

    void drawCard()
    {
        int cardDrawn = UnityEngine.Random.Range(0, deck.Count - 1);
        for (int i = 0; i < hand.Length; i++)
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
        for (int i = 0; i < usedCards; i++)
        {
            drawCard();
        }
        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i] != null)
            {
                cardButtons[i].SetActive(true);
            }
        }
        usedCards = 0;
        currentMana = maxMana;
        nameCards();

        playerTurn = false;
    }

    public void nameCards()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            //Debug.Log(hand[i].cardName);
            buttonText[i].text = hand[i].cardName;
        }
    }

    public void playerDead()
    {
        playerHealthText.text = "Health: 0";
        for (int i = 0; i < hand.Length; i++)
        {
            cardButtons[i].SetActive(false);
            passButton.SetActive(false);
        }
        deathScreen.SetActive(true);
    }
}
