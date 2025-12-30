using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        float outgoingDmg = card.power;
        foreach (StatusEffect effect in attacker.statusEffects)
        {
            outgoingDmg = effect.ModifyOutgoingDamage(outgoingDmg);
        }
        target.TakeDamage(outgoingDmg, card.damageType, attacker);
    }
}
