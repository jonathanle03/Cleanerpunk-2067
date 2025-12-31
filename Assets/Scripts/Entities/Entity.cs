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
    public int force;
    public int shield;
    public int accuracy = 100;
    public int evasion = 100;
    public float criticalDamageMultiplier = 2.0f;

    [Header("Damage Resistances")]
    public DamageResistances damageResistances = new DamageResistances();

    [Header("Additional Info")]
    public bool isBoss;
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public List<CardData> deck = new List<CardData>();
    public bool moveable = true;
    public int staggerCD = 0;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    public StatusUIController uiController;

    [Header("UI Elements")]
    public GameObject healthBar;
    public float healthBarMaxWidth;
    public Vector3 healthBarStartPos;

    void Awake()
    {
        uiController = GetComponentInChildren<StatusUIController>();
    }

    virtual public void TakeDamage(CardData card, Entity attacker)
    {
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

        currentHealth -= Mathf.CeilToInt(incomingDmg);
        healthText.text = "Health: " + currentHealth;
        Debug.Log($"{entityName} takes {incomingDmg} {card.damageType}{(card.isPiercing ? " (PIERCING)" : "")} damage{(isCritical ? " (CRITICAL)" : "")}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log($"{entityName} is dead");
            EntityDead();
        }
    }

    public bool CalculateHit(int attackerAccuracy, int targetEvasion)
    {
        // Formula: Base 85% + ((Accuracy - Evasion) * 0.5%)
        // Each point of difference = 0.5% change in hit rate
        float hitChance = 85f + ((attackerAccuracy - targetEvasion) * 0.5f);

        // Clamp between 10% and 100% to prevent guaranteed misses
        hitChance = Mathf.Clamp(hitChance, 10f, 100f);

        float roll = Random.Range(0f, 100f);

        Debug.Log($"Hit Calculation: {attackerAccuracy} ACC vs {targetEvasion} EVA = {hitChance}% (rolled {roll})");

        return roll < hitChance;
    }

    public bool CalculateCritical(int attackerAccuracy, int targetEvasion)
    {
        // Formula: Base 5% + (Accuracy - Evasion) * 0.1%
        // At equal stats (100 vs 100): 5% crit rate
        // Each point of difference = 0.1% change (lesser influence than hit rate)
        // Example: 105 Acc vs 100 Eva = 5% + (5 * 0.1%) = 5.5%
        // Example: 110 Acc vs 100 Eva = 5% + (10 * 0.1%) = 6%
        // Example: 100 Acc vs 107 Eva = 5% + (-7 * 0.1%) = 4.3%

        int statDifference = attackerAccuracy - targetEvasion;
        float critChance = 5f + (statDifference * 0.1f);

        // Clamp for future modification, although right now very high crit rates require absurd ACC
        critChance = Mathf.Clamp(critChance, 0f, 100f);

        float roll = Random.Range(0f, 100f);

        Debug.Log($"Crit Calculation: {attackerAccuracy} ACC vs {targetEvasion} EVA = {critChance}% (rolled {roll})");

        return roll < critChance;
    }

    virtual protected void ShowMissIndicator()
    {
        // Override in Player/Enemy classes to show miss UI
    }

    public void ApplyStatusEffect(StatusEffect effect)
    {
        StatusEffect instance = Instantiate(effect);
        statusEffects.Add(instance);
        instance.ApplyEffect(this);
        uiController.UpdateStatusUI(statusEffects);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        StatusEffect instance = Instantiate(effect);
        statusEffects.Remove(effect);
        instance.OnExpire(this);
        uiController.UpdateStatusUI(statusEffects);
    }

    public abstract void EntityDead();

}
