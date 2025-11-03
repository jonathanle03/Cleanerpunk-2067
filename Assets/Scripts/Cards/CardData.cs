using System.Collections.Generic;
using UnityEngine;

public enum CardType { Offensive, Defensive, Support }
public enum CardRarity { Common, Uncommon, Rare }
public enum CardTarget { Enemy, AllEnemies, Self, None }

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;
    [TextArea] public string description;
    public Sprite art;
    public CardType type;
    public CardRarity rarity;
    public CardTarget target;

    [Header("Gameplay Info")]
    public int cost;
    public int damage;
    public List<CardEffect> effects;
}
