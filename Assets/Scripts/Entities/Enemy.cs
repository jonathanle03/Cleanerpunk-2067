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

    [Header("Card Popup")]
    [SerializeField] private EnemyCardPopup cardPopup;
    [SerializeField] private Vector3 popupOffset = new Vector3(0, 1.5f, 0);

    public bool isDead;
    private Coroutine healthBarCoroutine;

    // Battle Script System
    private BattleScript battleScript;
    private BattlePhase currentPhase;
    private float lastPhaseHpPercent = 100f;

    private void Start()
    {
        
    }

    private void Update()
    {
        healthText.text = "" + currentHealth;
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
        force = enemyData.force;
        shield = enemyData.shield;
        accuracy = enemyData.accuracy;
        evasion = enemyData.evasion;
        isBoss = enemyData.isBoss;
        deck = new List<CardData>(enemyData.cards);

        damageResistances = enemyData.damageResistances;

        // Initialize battle script system
        battleScript = enemyData.battleScript;
        if (battleScript != null && battleScript.enableBattleScript)
        {
            lastPhaseHpPercent = 100f;
            currentPhase = battleScript.GetCurrentPhase(lastPhaseHpPercent);
            if (currentPhase != null)
            {
                currentPhase.ResetPhase();
            }
        }

        SpriteRenderer thisrender = GetComponent<SpriteRenderer>();
        thisrender.sprite = art;

        currentHealth = maxHealth;
        healthBarMaxWidth = (int)healthBar.GetComponent<Transform>().localScale.x;
        healthBarStartPos = healthBar.GetComponent<Transform>().localPosition;
        healthText.text = "Health: " + currentHealth;
        SelectNextCard();
    }

    public void RandomCard()
    {
        enemyCard = deck[Random.Range(0, deck.Count)];
    }

    /// <summary>
    /// Select the next card based on battle script or fallback to random
    /// </summary>
    public void SelectNextCard()
    {
        // Check if HP phase has changed
        float currentHpPercent = (float)currentHealth / maxHealth * 100f;

        if (battleScript != null && battleScript.enableBattleScript)
        {
            BattlePhase newPhase = battleScript.GetCurrentPhase(currentHpPercent);

            // If phase changed, reset the new phase
            if (newPhase != currentPhase)
            {
                Debug.Log($"{entityName} entering new phase at {currentHpPercent}% HP");
                battleScript.OnPhaseChange(newPhase);
                currentPhase = newPhase;
                lastPhaseHpPercent = currentHpPercent;
            }

            // Select card using battle script
            CardData selectedCard = battleScript.SelectCard(currentPhase, deck);
            if (selectedCard != null)
            {
                enemyCard = selectedCard;
                return;
            }
        }

        // Fallback to random card from deck
        RandomCard();
    }

    /// <summary>
    /// Display card name popup above enemy
    /// </summary>
    public void ShowCardPopup()
    {
        if (cardPopup != null && enemyCard != null)
        {
            Vector3 popupPosition = transform.position + popupOffset;
            cardPopup.ShowCardName(enemyCard.cardName, popupPosition);
        }
    }


    public override void TakeDamage(CardData card, Entity attacker)
    {
        // Prevent dead enemies from taking damage
        if (isDead)
        {
            return;
        }

        // Neural damage always hits and never crits
        bool isNeuralDamage = (card.damageType == DamageType.Neural);

        // Calculate hit chance (skip for Neural damage)
        if (!isNeuralDamage && !CalculateHit(attacker.accuracy, this.evasion))
        {
            Debug.Log($"{attacker.entityName}'s attack missed {entityName}!");
            ShowMissIndicator();
            return;
        }

        // Calculate base damage using new damage system
        float incomingDmg = DamageCalculator.CalculateDamage(card.damageType, card.isPiercing, card.power, card.globalDamageMultiplier, attacker, this);

        // Apply status effect modifiers
        foreach (StatusEffect effect in statusEffects)
        {
            incomingDmg = effect.ModifyIncomingDamage(incomingDmg);
        }

        // Calculate critical hit (Neural damage cannot crit)
        bool isCritical = false;
        if (!isNeuralDamage)
        {
            isCritical = CalculateCritical(attacker.accuracy, this.evasion);
            if (isCritical)
            {
                incomingDmg *= attacker.criticalDamageMultiplier;
                Debug.Log($"CRITICAL HIT! {attacker.criticalDamageMultiplier}x damage");
            }
        }

        // Apply damage variance
        incomingDmg = DamageCalculator.ApplyDamageVariance(incomingDmg);

        int damageDealt = Mathf.CeilToInt(incomingDmg);
        currentHealth -= damageDealt;
        healthText.text = "Health: " + currentHealth;
        Debug.Log($"{entityName} takes {incomingDmg} {card.damageType}{(card.isPiercing ? " (PIERCING)" : "")} damage{(isCritical ? " (CRITICAL)" : "")}");

        // Show damage popup via SpawnsDamagePopups singleton
        if (SpawnsDamagePopups.Instance != null)
        {
            SpawnsDamagePopups.Instance.DamageDone(damageDealt, transform.position, isCritical, card);
        }

        // Animate health bar
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }
        healthBarCoroutine = StartCoroutine(AnimateHealthBar());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log($"{entityName} is dead");
            EntityDead();
        }

        StartCoroutine(turnRed());
    }

    protected override void ShowMissIndicator()
    {
        // Show miss popup via SpawnsDamagePopups singleton
        if (SpawnsDamagePopups.Instance != null)
        {
            SpawnsDamagePopups.Instance.ShowMiss(transform.position);
        }
    }


    IEnumerator turnRed()
    {
        sprite.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(_timing);
        sprite.color = Color.white; 
    }

    IEnumerator AnimateHealthBar()
    {
        float targetScale = (float)currentHealth / maxHealth * healthBarMaxWidth;
        Vector3 startScale = healthBar.GetComponent<Transform>().localScale;
        Vector3 targetScaleVector = new Vector3(targetScale, startScale.y, startScale.z);
        
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Lerp the scale
            healthBar.GetComponent<Transform>().localScale = Vector3.Lerp(startScale, targetScaleVector, t);

            // Update position to keep left-aligned
            float currentScaleX = healthBar.GetComponent<Transform>().localScale.x;
            healthBar.GetComponent<Transform>().localPosition = new Vector3(
                healthBarStartPos.x - (healthBarMaxWidth - currentScaleX) / 2 * healthBar.GetComponent<RectTransform>().rect.width,
                healthBarStartPos.y,
                healthBarStartPos.z
            );

            yield return null;
        }

        // Ensure final values are exact
        healthBar.GetComponent<Transform>().localScale = targetScaleVector;
        healthBar.GetComponent<Transform>().localPosition = new Vector3(
            healthBarStartPos.x - (healthBarMaxWidth - targetScale) / 2 * healthBar.GetComponent<RectTransform>().rect.width,
            healthBarStartPos.y,
            healthBarStartPos.z
        );
    }

}
