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

    virtual public void TakeDamage(float cardPower, DamageType damageType, Entity attacker)
    {
        // Neural damage types bypass ACC/EVA and always hit
        bool isNeuralDamage = (damageType == DamageType.Neural || damageType == DamageType.PiercingNeural);

        // Calculate hit chance (skip for Neural damage types)
        if (!isNeuralDamage && !CalculateHit(attacker.accuracy, this.evasion))
        {
            Debug.Log($"{attacker.entityName}'s attack missed {entityName}!");
            ShowMissIndicator();
            return;
        }

        // Calculate base damage using new damage system
        float incomingDmg = DamageCalculator.CalculateDamage(damageType, cardPower, attacker, this);

        // Apply status effect modifiers
        foreach (StatusEffect effect in statusEffects)
        {
            incomingDmg = effect.ModifyIncomingDamage(incomingDmg);
        }

        // Calculate critical hit (Neural damage types cannot crit)
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
        Debug.Log($"{entityName} takes {incomingDmg} {damageType} damage{(isCritical ? " (CRITICAL)" : "")}");

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

        // Clamp between 5% and 97% to prevent guaranteed hits/misses
        hitChance = Mathf.Clamp(hitChance, 5f, 97f);

        float roll = Random.Range(0f, 100f);

        Debug.Log($"Hit Calculation: {attackerAccuracy} ACC vs {targetEvasion} EVA = {hitChance}% (rolled {roll})");

        return roll < hitChance;
    }

    public bool CalculateCritical(int attackerAccuracy, int targetEvasion)
    {
        // Formula: Base 6% + (Accuracy - Evasion) * 0.1%
        // At equal stats (100 vs 100): 6% crit rate
        // Each point of difference = 0.1% change (lesser influence than hit rate)
        // Example: 105 Acc vs 100 Eva = 6% + (5 * 0.1%) = 6.5%
        // Example: 110 Acc vs 100 Eva = 6% + (10 * 0.1%) = 7%
        // Example: 100 Acc vs 107 Eva = 6% + (-7 * 0.1%) = 5.3%

        int statDifference = attackerAccuracy - targetEvasion;
        float critChance = 6f + (statDifference * 0.1f);

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
