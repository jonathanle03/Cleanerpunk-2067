using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        // Apply status effect modifiers to card power
        float modifiedPower = card.power;
        foreach (StatusEffect effect in attacker.statusEffects)
        {
            modifiedPower = effect.ModifyOutgoingDamage(modifiedPower);
        }

        // Create a modified card with the new power
        CardData modifiedCard = Instantiate(card);
        modifiedCard.power = modifiedPower;

        target.TakeDamage(modifiedCard, attacker);
    }
}
