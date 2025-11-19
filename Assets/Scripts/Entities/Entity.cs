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

    [Header("Additional Info")]
    public bool isBoss;
    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public List<CardData> deck = new List<CardData>();
    public bool moveable = true;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    public StatusUIController uiController;

    void Awake()
    {
        uiController = GetComponentInChildren<StatusUIController>();
    }

    public void TakeDamage(float cardPower, int incomingAttack)
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
            Debug.Log($"{entityName} is dead");
            EntityDead();
        }
    }

    public void ApplyStatusEffect(StatusEffect effect)
    {
        StatusEffect instance = Instantiate(effect);
        statusEffects.Add(instance);
        effect.ApplyEffect(this);
        uiController.UpdateStatusUI(statusEffects);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        StatusEffect instance = Instantiate(effect);
        statusEffects.Remove(effect);
        effect.OnExpire(this);
        uiController.UpdateStatusUI(statusEffects);
    }

    public abstract void EntityDead();

}
