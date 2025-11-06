using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public override void Execute(CardData card, Player player, Enemy enemy, CardTarget target)
    {
        if (target == CardTarget.Enemy)
        {
            enemy.TakeDamage(card.power, player.attack);
        }
        else if (target == CardTarget.Player)
        {
            player.takeDamage(card.power, enemy.attack);
        }
    }
}
