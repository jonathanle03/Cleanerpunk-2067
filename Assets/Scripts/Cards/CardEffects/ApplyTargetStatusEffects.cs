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
                if (target is Player && ((Player) target).isInvisible)
                {
                    continue;
                }
                target.ApplyStatusEffect(statusEffect);
            }
        }
    }
}
