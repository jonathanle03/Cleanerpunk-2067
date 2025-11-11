using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public override void Execute(CardData card, Player player, Enemy enemy)
    {
        if (card.target == CardTarget.Enemy)
        {
            enemy.TakeDamage(card.power, player.attack);
        }
        else if (card.target == CardTarget.Player)
        {
            player.takeDamage(card.power, enemy.attack);
        }
    }
}
