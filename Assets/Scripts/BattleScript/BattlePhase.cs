using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeightedCard
{
    public CardData card;
    [Tooltip("Selection weight (higher = more likely to be chosen)")]
    [Min(0.01f)]
    public float weight = 1f;
}

[System.Serializable]
public class BattlePhase
{
    [Header("Phase Configuration")]
    [Tooltip("HP percentage threshold to activate this phase (0-100)")]
    [Range(0, 100)]
    public float hpThresholdPercent = 100f;

    [Header("Card Play Mode")]
    public PhasePlayMode playMode = PhasePlayMode.Random;

    [Header("Random Mode Settings")]
    [Tooltip("Weighted cards for random selection (used in Random mode)")]
    public List<WeightedCard> weightedRandomCards = new List<WeightedCard>();

    [Header("Linear Mode Settings")]
    [Tooltip("Cards played in order (used in Linear mode)")]
    public List<CardData> linearCards = new List<CardData>();

    [Header("Sequence + Loop Mode Settings")]
    [Tooltip("Cards played once in sequence before entering loop")]
    public List<CardData> sequenceCards = new List<CardData>();
    [Tooltip("Cards that loop after sequence is complete")]
    public List<CardData> loopCards = new List<CardData>();

    [Header("Special Turn-Based Card")]
    public bool enableSpecialTurnCard = false;
    [Tooltip("Play special card every X turns")]
    public int specialCardTurnInterval = 3;
    public SpecialCardMode specialCardMode = SpecialCardMode.Preselected;
    [Tooltip("Preselected card for special turns")]
    public CardData specialCard;
    [Tooltip("Random cards for special turns (picks one randomly)")]
    public List<CardData> specialRandomCards = new List<CardData>();

    // Runtime tracking
    [System.NonSerialized] public int linearCardIndex = 0;
    [System.NonSerialized] public bool sequenceComplete = false;
    [System.NonSerialized] public int loopCardIndex = 0;
    [System.NonSerialized] public int turnCounter = 0;

    public void ResetPhase()
    {
        linearCardIndex = 0;
        sequenceComplete = false;
        loopCardIndex = 0;
        turnCounter = 0;
    }
}

public enum PhasePlayMode
{
    Random,          // Play random cards from randomCards list
    Linear,          // Play cards in order from linearCards list
    SequenceLoop     // Play sequenceCards once, then loop through loopCards
}

public enum SpecialCardMode
{
    Preselected,     // Use the single preselected card
    RandomFromPhase  // Use random card from the current phase's available cards
}
