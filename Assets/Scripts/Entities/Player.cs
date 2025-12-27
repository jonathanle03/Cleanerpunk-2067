using UnityEngine;
using System.Collections;
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
    [SerializeField] GameObject cancelButton;
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
        accuracy = stats.accuracy;
        evasion = stats.evasion;
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
            
            if (hand[pick].target == CardTarget.Enemy)
            {
                cancelButton.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    enemyManager.enemyButtons[i].SetActive(true);
                }

                chosenCard = pick;
            }
            else if (hand[pick].target == CardTarget.AllEnemies)
            {
                // Show cancel button and all enemy buttons for confirmation
                cancelButton.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    enemyManager.enemyButtons[i].SetActive(true);
                }
                chosenCard = pick;
            }
            else if (hand[pick].target == CardTarget.Self)
            {
                // If clicking the same card again, play it
                if (selectedCard == pick)
                {
                    StartCoroutine(playSelfCardCoroutine(pick));
                }
                else
                {
                    // Show cancel button for confirmation
                    cancelButton.SetActive(true);
                    chosenCard = pick;
                }
            }

            updateMana();
        }
    }

    public void enemySelect(int pick)
    {
        cancelButton.SetActive(false);
        if(special && selectedCard == 6)
        {
            if(currentMana < stats.maxMana)
            {
                currentMana += 1;
            }
            StartCoroutine(attackStatic.doEffectWithDelay(this, enemyManager.enemies[pick].GetComponent<Enemy>()));
            attackStaticButton.SetActive(true);
            defenseStaticButton.SetActive(true);
            passButton.SetActive(true);
            for (int k = 0; k < 6; k++)
            {
                cardButtons[k].SetActive(true);
            }
            fillHand();

        }
        else if (hand[selectedCard].target == CardTarget.AllEnemies)
        {
            // Play AllEnemies card (any enemy button confirms)
            StartCoroutine(playAllEnemiesCardCoroutine(selectedCard));
        }
        else
        {
            // Single enemy target card
            StartCoroutine(playSingleEnemyCardCoroutine(selectedCard, pick));
        }
        special = false;
    }

    private IEnumerator playSingleEnemyCardCoroutine(int cardIndex, int enemyIndex)
    {
        currentMana -= hand[cardIndex].cost;
        updateMana();

        yield return StartCoroutine(hand[cardIndex].doEffectWithDelay(this, enemyManager.enemies[enemyIndex].GetComponent<Enemy>()));

        for(int k = 0; k < 6; k++)
        {
            if (hand[k] != null)
            {
                cardButtons[k].SetActive(true);
            }
        }

        discards.Add(hand[cardIndex]);
        cooldowns.Add(hand[cardIndex].maxCooldown);
        cardButtons[chosenCard].SetActive(false);
        hand[cardIndex] = null;
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

    public override void TakeDamage(float cardPower, int incomingAttack, Entity attacker)
    {
        // Calculate hit chance
        if (!CalculateHit(attacker.accuracy, this.evasion))
        {
            Debug.Log($"{attacker.entityName}'s attack missed {entityName}!");
            ShowMissIndicator();
            return;
        }

        // Calculate base damage
        float incomingDmg = cardPower * incomingAttack * 100f / (100f + defense);
        foreach (StatusEffect effect in statusEffects)
        {
            incomingDmg = effect.ModifyIncomingDamage(incomingDmg);
        }

        // Calculate critical hit
        bool isCritical = CalculateCritical(attacker.accuracy, this.evasion);
        if (isCritical)
        {
            incomingDmg *= attacker.criticalDamageMultiplier;
            Debug.Log($"CRITICAL HIT! {attacker.criticalDamageMultiplier}x damage");
        }

        int damageDealt = Mathf.CeilToInt(incomingDmg);
        currentHealth -= damageDealt;
        healthText.text = "" + currentHealth;
        Debug.Log($"{entityName} takes {incomingDmg} damage{(isCritical ? " (CRITICAL)" : "")}");

        // Show damage popup via SpawnsDamagePopups singleton
        if (SpawnsDamagePopups.Instance != null)
        {
            SpawnsDamagePopups.Instance.DamageDone(damageDealt, transform.position, isCritical);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log($"{entityName} is dead");
            EntityDead();
        }
    }

    protected override void ShowMissIndicator()
    {
        // Show miss popup via SpawnsDamagePopups singleton
        if (SpawnsDamagePopups.Instance != null)
        {
            SpawnsDamagePopups.Instance.ShowMiss(transform.position);
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
        StartCoroutine(defenseStatic.doEffectWithDelay(this, this));
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

    public void cancelChoice()
    {
        for (int k = 0; k < 6; k++)
        {
            // Only show card buttons for cards that haven't been played yet
            if (hand[k] != null)
            {
                cardButtons[k].SetActive(true);
            }
        }
        cancelButton.SetActive(false);
    }

    /*private void playAllEnemiesCard(int cardIndex)
    {
        // Consume mana
        currentMana -= hand[cardIndex].cost;
        updateMana();

        // Apply effect to all living enemies
        for (int i = 0; i < enemyManager.enemies.Length; i++)
        {
            Enemy targetEnemy = enemyManager.enemies[i].GetComponent<Enemy>();
            if (targetEnemy != null && !targetEnemy.isDead)
            {
                hand[cardIndex].doEffect(this, targetEnemy);
            }
        }

        // Hide cancel button
        cancelButton.SetActive(false);

        // Show remaining cards
        for (int k = 0; k < 6; k++)
        {
            if (k != cardIndex && hand[k] != null)
            {
                cardButtons[k].SetActive(true);
            }
        }

        // Discard card
        discards.Add(hand[cardIndex]);
        cooldowns.Add(hand[cardIndex].maxCooldown);
        cardButtons[cardIndex].SetActive(false);
        hand[cardIndex] = null;
        usedCards++;
    }*/

    private IEnumerator playAllEnemiesCardCoroutine(int cardIndex)
    {
        // Consume mana
        currentMana -= hand[cardIndex].cost;
        updateMana();

        // Apply effect to all living enemies with delays
        for (int i = 0; i < enemyManager.enemies.Length; i++)
        {
            Enemy targetEnemy = enemyManager.enemies[i].GetComponent<Enemy>();
            if (targetEnemy != null && !targetEnemy.isDead)
            {
                yield return StartCoroutine(hand[cardIndex].doEffectWithDelay(this, targetEnemy));
            }
        }

        // Hide cancel button
        cancelButton.SetActive(false);

        // Show remaining cards
        for (int k = 0; k < 6; k++)
        {
            if (k != cardIndex && hand[k] != null)
            {
                cardButtons[k].SetActive(true);
            }
        }

        // Discard card
        discards.Add(hand[cardIndex]);
        cooldowns.Add(hand[cardIndex].maxCooldown);
        cardButtons[cardIndex].SetActive(false);
        hand[cardIndex] = null;
        usedCards++;
    }

    /*private void playSelfCard(int cardIndex)
    {
        // Consume mana
        currentMana -= hand[cardIndex].cost;
        updateMana();

        // Apply effect to self
        hand[cardIndex].doEffect(this, this);

        // Hide enemy buttons

        // Hide cancel button
        cancelButton.SetActive(false);

        // Show remaining cards
        for (int k = 0; k < 6; k++)
        {
            if (k != cardIndex && hand[k] != null)
            {
                cardButtons[k].SetActive(true);
            }
        }

        // Discard card
        discards.Add(hand[cardIndex]);
        cooldowns.Add(hand[cardIndex].maxCooldown);
        cardButtons[cardIndex].SetActive(false);
        hand[cardIndex] = null;
        usedCards++;
    }*/

    private IEnumerator playSelfCardCoroutine(int cardIndex)
    {
        // Consume mana
        currentMana -= hand[cardIndex].cost;
        updateMana();

        // Apply effect to self with delays
        yield return StartCoroutine(hand[cardIndex].doEffectWithDelay(this, this));

        // Hide cancel button
        cancelButton.SetActive(false);

        // Show remaining cards
        for (int k = 0; k < 6; k++)
        {
            if (k != cardIndex && hand[k] != null)
            {
                cardButtons[k].SetActive(true);
            }
        }

        // Discard card
        discards.Add(hand[cardIndex]);
        cooldowns.Add(hand[cardIndex].maxCooldown);
        cardButtons[cardIndex].SetActive(false);
        hand[cardIndex] = null;
        usedCards++;
    }
}
