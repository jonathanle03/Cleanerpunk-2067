using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public override void Execute(Card card, Player player, Enemy target)
    {
        target.TakeDamage(card.damage);
    }
}
