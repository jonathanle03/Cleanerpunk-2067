using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Apply Self Status Effects")]
public class ApplySelfStatusEffectsEffect : CardEffect
{
    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        foreach (StatusEffect statusEffect in card.selfStatusEffects)
        {
            if (Random.value <= statusEffect.applicationChance)
            {
                attacker.ApplyStatusEffect(statusEffect);
            }
        }
    }
}
