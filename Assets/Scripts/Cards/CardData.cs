using System.Collections.Generic;
using UnityEngine;

public enum CardType { Offensive, Defensive, Support }
public enum CardRarity { Common, Uncommon, Rare }
public enum CardTarget { Enemy, AllEnemies, Self }

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card Data")]
[System.Serializable]
public class CardData : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;
    [TextArea] public string description;
    public Texture art;
    public CardType type;
    public CardRarity rarity;
    public CardTarget target;

    [Header("Gameplay Info")]
    public int cost;
    public float power;
    public int maxCooldown;
    public List<CardEffect> effects;
    public List<StatusEffect> targetStatusEffects;
    public List<StatusEffect> selfStatusEffects;

    public void doEffect(Entity attacker, Entity target)
    {
        foreach (CardEffect effect in effects)
        {
            effect.Execute(this, attacker, target);
        }
    }
}
