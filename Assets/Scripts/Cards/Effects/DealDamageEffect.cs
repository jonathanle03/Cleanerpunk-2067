using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public override void Execute(CardData card, Player player, Enemy target)
    {
        target.TakeDamage(card.damage);
    }
}
