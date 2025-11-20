using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UI;

public class Player : Entity
{
    [Header("UI Elements")]
    [SerializeField] GameObject deathScreen;
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] GameObject[] cardButtons = new GameObject[6];
    [SerializeField] TextMeshProUGUI[] buttonText = new TextMeshProUGUI[6];
    [SerializeField] GameObject passButton;
    private int chosenCard;

    [Header("Class References")]
    public PlayerStats stats;
    [SerializeField] Enemy enemy;
    [SerializeField] EnemyManager enemyManager;

    public bool playerTurn = true;
    public bool isInvisible = false;


    [Header("Cards")]
    [SerializeField] CardData[] hand = new CardData[6];
    [SerializeField] List<CardData> discards = new List<CardData>();
    [SerializeField] List<int> cooldowns = new List<int>(); 
    private int selectedCard;
    private int usedCards;

    [Header("Stats")]
    public const int maxMana = 9;
    public int currentMana;
    void Start()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        currentHealth = stats.maxHealth;
        healthText.text = "Health: " + currentHealth;
        currentMana = stats.maxEnergy;

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
        healthText.text = "Health: " + currentHealth;
    }

    public void useCard(int pick)
    {
        Debug.Log("use card activated, pick number: " + pick); 
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
            else if (hand[pick].target == CardTarget.AllEnemies)
            {
                for (int i = 0; i < enemyManager.enemies.Length; i++)
                {
                    hand[pick].doEffect(this, enemyManager.enemies[i].GetComponent<Enemy>());
                }
                
                discards.Add(hand[pick]);
                cooldowns.Add(hand[pick].maxCooldown);

                cardButtons[pick].SetActive(false);
                hand[pick] = null;
                usedCards++;
            }
            else if (hand[pick].target == CardTarget.Self)
            {
                hand[pick].doEffect(this, this);
                
      
                discards.Add(hand[pick]);
                cooldowns.Add(hand[pick].maxCooldown);

                cardButtons[pick].SetActive(false);
                hand[pick] = null;
                usedCards++;
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
        cooldowns.Add(hand[selectedCard].maxCooldown);
        //discardCard(hand[selectedCard]);

        cardButtons[chosenCard].SetActive(false);
        hand[selectedCard] = null;
        usedCards++;

    }

    void changeCooldowns()
    {
        for (int i = cooldowns.Count - 1; i >= 0; i--)
        {
            cooldowns[i] -= 1;

            if (cooldowns[i] <= 0)
            {
                cooldowns.RemoveAt(i);
                CardData restoredCard = discards[i];
                deck.Add(restoredCard);
                discards.RemoveAt(i);
            }
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
        for (int i = statusEffects.Count - 1; i >= 0; --i)
        {
            statusEffects[i].OnTurnEnd(this);
        }

        if (staggerCD > 0)
        {
            staggerCD--;
        }

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
        changeCooldowns();
        playerTurn = false;
    }

    public void nameCards()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            //Debug.Log(hand[i].cardName);
            cardButtons[i].GetComponent<cardReader>().card = hand[i];
            cardButtons[i].GetComponent<cardReader>().setCard(); 
        }
    }

    public override void EntityDead()
    {
        healthText.text = "Health: 0";
        for (int i = 0; i < hand.Length; i++)
        {
            cardButtons[i].SetActive(false);
            passButton.SetActive(false);
        }
        deathScreen.SetActive(true);
    }
}
