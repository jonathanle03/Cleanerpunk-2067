using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        target.TakeDamage(card.power, attacker.attack);
    }
}
