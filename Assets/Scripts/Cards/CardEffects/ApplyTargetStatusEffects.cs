using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Apply Target Status Effects")]
public class ApplyTargetStatusEffectsEffect : CardEffect
{
    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        foreach (StatusEffect statusEffect in card.targetStatusEffects)
        {
            if (Random.value <= statusEffect.applicationChance)
            {
                target.ApplyStatusEffect(statusEffect);
            }
        }
    }
}
