using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Apply Status Effects")]
public class ApplyStatusEffectsEffect : CardEffect
{
    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        foreach (StatusEffect statusEffect in card.statusEffects)
        {
            if (Random.value <= statusEffect.applicationChance)
            {
                target.ApplyStatusEffect(statusEffect);
            }
        }
    }
}
