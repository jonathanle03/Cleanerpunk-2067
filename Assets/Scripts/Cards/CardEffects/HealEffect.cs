using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Heal")]
public class HealEffect : CardEffect
{
    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        attacker.currentHealth += Mathf.CeilToInt(card.power * attacker.maxHealth);
        if (attacker.currentHealth > attacker.maxHealth)
        {
            attacker.currentHealth = attacker.maxHealth;
        }
    }
}
