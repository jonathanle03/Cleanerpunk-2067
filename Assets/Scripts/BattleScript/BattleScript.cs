using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BattleScript", menuName = "Enemies/Battle Script")]
public class BattleScript : ScriptableObject
{
    [Header("Battle Script Configuration")]
    [Tooltip("Enable battle script system for this enemy")]
    public bool enableBattleScript = true;

    [Tooltip("Phases sorted by HP threshold (highest to lowest)")]
    public List<BattlePhase> phases = new List<BattlePhase>();

    /// <summary>
    /// Get the appropriate phase based on current HP percentage
    /// </summary>
    public BattlePhase GetCurrentPhase(float currentHpPercent)
    {
        if (!enableBattleScript || phases.Count == 0)
            return null;

        // Find the first phase where HP is above or equal to threshold
        // Phases should be ordered from highest threshold to lowest
        for (int i = 0; i < phases.Count; i++)
        {
            if (currentHpPercent >= phases[i].hpThresholdPercent)
            {
                return phases[i];
            }
        }

        // If no phase matches, return the last phase (lowest threshold)
        return phases[phases.Count - 1];
    }

    /// <summary>
    /// Select the next card based on the current phase's rules
    /// </summary>
    public CardData SelectCard(BattlePhase phase, List<CardData> fallbackDeck)
    {
        if (phase == null || !enableBattleScript)
        {
            // Fallback to random card from deck
            if (fallbackDeck != null && fallbackDeck.Count > 0)
                return fallbackDeck[Random.Range(0, fallbackDeck.Count)];
            return null;
        }
        phase.turnCounter++;

        // Check if it's time for a special turn card
        if (phase.enableSpecialTurnCard && 
            phase.turnCounter % phase.specialCardTurnInterval == 0)
        {
            CardData specialCard = GetSpecialCard(phase);
            if (specialCard != null)
                return specialCard;
        }

        // Select card based on play mode
        switch (phase.playMode)
        {
            case PhasePlayMode.Random:
                return GetRandomCard(phase);
            case PhasePlayMode.Linear:
                return GetLinearCard(phase);
            case PhasePlayMode.SequenceLoop:
                return GetSequenceLoopCard(phase);
            default:
                return GetRandomCard(phase);
        }
    }

    private CardData GetSpecialCard(BattlePhase phase)
    {
        switch (phase.specialCardMode)
        {
            case SpecialCardMode.Preselected:
                return phase.specialCard;

            case SpecialCardMode.RandomFromPhase:
                // Get random card from current phase's available cards
                List<CardData> availableCards = GetAvailableCardsForPhase(phase);
                if (availableCards.Count > 0)
                    return availableCards[Random.Range(0, availableCards.Count)];
                break;
        }
        return null;
    }

    private CardData GetRandomCard(BattlePhase phase)
    {
        if (phase.weightedRandomCards.Count == 0)
            return null;

        // Calculate total weight
        float totalWeight = 0f;
        foreach (WeightedCard wc in phase.weightedRandomCards)
        {
            if (wc.card != null)
                totalWeight += wc.weight;
        }

        if (totalWeight <= 0f)
            return null;

        // Pick a random value between 0 and total weight
        float randomValue = Random.Range(0f, totalWeight);

        // Find which card corresponds to this value
        float cumulativeWeight = 0f;
        foreach (WeightedCard wc in phase.weightedRandomCards)
        {
            if (wc.card == null)
                continue;

            cumulativeWeight += wc.weight;
            if (randomValue <= cumulativeWeight)
            {
                return wc.card;
            }
        }

        // Fallback (should not reach here)
        return phase.weightedRandomCards[0].card;
    }

    private CardData GetLinearCard(BattlePhase phase)
    {
        if (phase.linearCards.Count == 0)
            return null;

        CardData card = phase.linearCards[phase.linearCardIndex];
        phase.linearCardIndex = (phase.linearCardIndex + 1) % phase.linearCards.Count;
        return card;
    }

    private CardData GetSequenceLoopCard(BattlePhase phase)
    {
        // First play through sequence cards
        if (!phase.sequenceComplete && phase.sequenceCards.Count > 0)
        {
            int index = phase.linearCardIndex;
            if (index < phase.sequenceCards.Count)
            {
                CardData card = phase.sequenceCards[index];
                phase.linearCardIndex++;
                
                // Check if sequence is complete
                if (phase.linearCardIndex >= phase.sequenceCards.Count)
                {
                    phase.sequenceComplete = true;
                    phase.loopCardIndex = 0;
                }
                return card;
            }
        }

        // Then loop through loop cards
        if (phase.loopCards.Count > 0)
        {
            CardData card = phase.loopCards[phase.loopCardIndex];
            phase.loopCardIndex = (phase.loopCardIndex + 1) % phase.loopCards.Count;
            return card;
        }
        return null;
    }

    private List<CardData> GetAvailableCardsForPhase(BattlePhase phase)
    {
        List<CardData> cards = new List<CardData>();

        switch (phase.playMode)
        {
            case PhasePlayMode.Random:
                foreach (WeightedCard wc in phase.weightedRandomCards)
                {
                    if (wc.card != null)
                        cards.Add(wc.card);
                }
                break;
            case PhasePlayMode.Linear:
                cards.AddRange(phase.linearCards);
                break;
            case PhasePlayMode.SequenceLoop:
                if (!phase.sequenceComplete)
                    cards.AddRange(phase.sequenceCards);
                else
                    cards.AddRange(phase.loopCards);
                break;
        }
        return cards;
    }

    /// <summary>
    /// Reset all phase tracking when phase changes
    /// </summary>
    public void OnPhaseChange(BattlePhase newPhase)
    {
        if (newPhase != null)
        {
            newPhase.ResetPhase();
        }
    }
}
