using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    [Tooltip("Delay in seconds before executing this effect (for animation sync)")]
    public float delayBeforeEffect = 0f;

    public abstract void Execute(CardData card, Entity attacker, Entity target);
}
