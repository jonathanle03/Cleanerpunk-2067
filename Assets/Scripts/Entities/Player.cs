using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Player : Entity
{
    [Header("UI Elements")]
    [SerializeField] GameObject deathScreen;
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] GameObject[] currentManaIcons = new GameObject[9];
    [SerializeField] GameObject[] maxManaIcons = new GameObject[9];
    [SerializeField] GameObject[] cardButtons = new GameObject[6];
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

    [SerializeField] CardData attackStatic;
    [SerializeField] CardData defenseStatic;
    private bool special;

    [SerializeField] GameObject attackStaticButton;
    [SerializeField] GameObject defenseStaticButton;
 
    [Header("Stats")]
    public int maxMana;
    public int currentMana;

    void Start()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        maxHealth = stats.maxHealth;
        currentHealth = stats.maxHealth;
        healthBarMaxWidth = (int)healthBar.GetComponent<Transform>().localScale.x;
        healthBarStartPos = healthBar.GetComponent<Transform>().localPosition;
        attack = stats.attack;
        defense = stats.defense;
        maxMana = stats.maxMana;
        currentMana = stats.maxMana;

        updateMana();
        for (int i = 0; i < maxMana; i++)
        {
            maxManaIcons[i].SetActive(true);
        }
        for (int i = 8; i >= maxMana; i--)
        {
            maxManaIcons[i].SetActive(false);
        }

        deck = stats.getDeck();

        for (int i = 0; i < 6; i++)
        {
            hand[i] = deck[0];
            deck.RemoveAt(0);
        }

        nameCards();
    }

    private void Update()
    {
        energyText.text = "" + currentMana;
        healthText.text = "" + currentHealth;
        healthBar.GetComponent<Transform>().localScale = new Vector3((float)currentHealth / maxHealth * healthBarMaxWidth, healthBar.GetComponent<Transform>().localScale.y, healthBar.GetComponent<Transform>().localScale.z);
        healthBar.GetComponent<Transform>().localPosition = new Vector3(healthBarStartPos.x - (healthBarMaxWidth - healthBar.GetComponent<Transform>().localScale.x) / 2 * healthBar.GetComponent<RectTransform>().rect.width, healthBarStartPos.y, healthBarStartPos.z);
    }

    public void useCard(int pick)
    {
        Debug.Log("use card activated, pick number: " + pick); 
        if (currentMana >= hand[pick].cost)
        {

            for(int k = 0; k < 6; k++)
            {
                cardButtons[k].SetActive(false);
            }
            
            selectedCard = pick;
            //currentMana -= hand[pick].cost;
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
                currentMana -= hand[pick].cost;
                for (int i = 0; i < enemyManager.enemies.Length; i++)
                {
                    hand[pick].doEffect(this, enemyManager.enemies[i].GetComponent<Enemy>());
                }

                for (int k = 0; k < 6; k++)
                {
                    if(k != pick)
                    {
                        cardButtons[k].SetActive(true);
                    }
                    
                }

                discards.Add(hand[pick]);
                cooldowns.Add(hand[pick].maxCooldown);

                cardButtons[pick].SetActive(false);
                hand[pick] = null;
                usedCards++;
            }
            else if (hand[pick].target == CardTarget.Self)
            {
                currentMana -= hand[pick].cost;
                hand[pick].doEffect(this, this);
                
      
                discards.Add(hand[pick]);
                cooldowns.Add(hand[pick].maxCooldown);

                cardButtons[pick].SetActive(false);
                hand[pick] = null;
                usedCards++;

                for (int k = 0; k < 6; k++)
                {
                    if(k != pick && hand[k] != null)
                    {
                        cardButtons[k].SetActive(true);
                    }
                    
                }
            }

            updateMana();
        }
    }

    public void enemySelect(int pick)
    {
        if(special && selectedCard == 6)
        {
            if(currentMana < stats.maxMana)
            {
                currentMana += 1;
            }
            attackStatic.doEffect(this, enemyManager.enemies[pick].GetComponent<Enemy>());
            attackStaticButton.SetActive(true);
            defenseStaticButton.SetActive(true);
            passButton.SetActive(true);
            for (int k = 0; k < 6; k++)
            {
                cardButtons[k].SetActive(true);
            }
            fillHand();

        }else
        {
            currentMana -= hand[selectedCard].cost;
        updateMana();
                hand[selectedCard].doEffect(this, enemyManager.enemies[pick].GetComponent<Enemy>());
                for (int i = 0; i < 3; i++)
                {
                    enemyManager.enemyButtons[i].SetActive(false);
                }
                for(int k = 0; k < 6; k++)
                {
                    if (hand[k] != null)
                    {
                        cardButtons[k].SetActive(true);

                    }
                }

                discards.Add(hand[selectedCard]);
                cooldowns.Add(hand[selectedCard].maxCooldown);
                //discardCard(hand[selectedCard]);

                cardButtons[chosenCard].SetActive(false);
                hand[selectedCard] = null;
                usedCards++;

        }
        special = false;
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
        int cardDrawn = Random.Range(0, deck.Count - 1);
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

        if( currentMana < stats.maxMana)
        {
            currentMana += 2;
        }
        
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        updateMana();

        nameCards();
        changeCooldowns();
        playerTurn = false;
    }

    public void nameCards()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            cardButtons[i].GetComponent<cardReader>().card = hand[i];
            cardButtons[i].GetComponent<cardReader>().setCard(); 
        }
    }

    public void updateMana()
    {
        for (int i = 0; i < currentMana; i++)
        {
            currentManaIcons[i].SetActive(true);
        }
        for (int i = 8; i >= currentMana; i--)
        {
            currentManaIcons[i].SetActive(false);
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

    public void staticAttack()
    {
        for(int i = 0; i < 3; i++)
        {
            enemyManager.enemyButtons[i].SetActive(true);
        }
        for(int k = 0; k < 6; k++)
        {
            cardButtons[k].SetActive(false);
        }
        attackStaticButton.SetActive(false);
        defenseStaticButton.SetActive(false);
        passButton.SetActive(false);
        special = true;
        selectedCard = 6;

    }

    public void staticDefense()
    {
        if(currentMana < maxMana)
        {
            currentMana += 2;
        }
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        defenseStatic.doEffect(this, this);
        fillHand();
    }

    public void charge()
    {
        if (currentMana < maxMana)
        {
            currentMana += 4;
        }
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        fillHand();
    }
}
