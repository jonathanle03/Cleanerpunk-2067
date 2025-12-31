using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Detonate Self Damage")]
public class DetonateSelfDamageEffect : CardEffect
{
    [Header("Detonate Settings")]
    [Tooltip("Damage dealt to the attacker (self)")]
    public float selfDamageMultiplier = 1.0f;

    public override void Execute(CardData card, Entity attacker, Entity target)
    {
        // Apply status effect modifiers to card power for outgoing damage
        float modifiedPower = card.power;
        foreach (StatusEffect effect in attacker.statusEffects)
        {
            modifiedPower = effect.ModifyOutgoingDamage(modifiedPower);
        }

        // Create a modified card with the new power
        CardData modifiedCard = Instantiate(card);
        modifiedCard.power = modifiedPower;

        // Deal damage to the primary target
        target.TakeDamage(modifiedCard, attacker);

        // Deal damage to all other entities in battle
        DamageAllOthersInBattle(modifiedCard, attacker, target);

        // Deal self-damage to the attacker
        DealSelfDamage(card, attacker);
    }

    private void DamageAllOthersInBattle(CardData card, Entity attacker, Entity primaryTarget)
    {
        // Find the EnemyManager in the scene
        EnemyManager enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogWarning("DetonateSelfDamageEffect: Could not find EnemyManager in scene");
            return;
        }

        // Damage all enemies (except the primary target if it's an enemy)
        foreach (GameObject enemyObj in enemyManager.enemies)
        {
            if (enemyObj == null) continue;

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null && !enemy.isDead && enemy != primaryTarget)
            {
                enemy.TakeDamage(card, attacker);
            }
        }

        // Damage the player (if the player is not the attacker)
        Player player = GameObject.FindFirstObjectByType<Player>();
        if (player != null && player != attacker)
        {
            player.TakeDamage(card, attacker);
        }
    }

    private void DealSelfDamage(CardData card, Entity attacker)
    {
        // Calculate self-damage
        float selfDamage = card.power * selfDamageMultiplier;

        // Apply outgoing damage modifiers from attacker's status effects
        foreach (StatusEffect effect in attacker.statusEffects)
        {
            selfDamage = effect.ModifyOutgoingDamage(selfDamage);
        }

        // Create a modified card for self-damage
        CardData selfDamageCard = Instantiate(card);
        selfDamageCard.power = selfDamage;

        // Deal damage to self
        attacker.TakeDamage(selfDamageCard, attacker);

        Debug.Log($"{attacker.entityName} detonates, dealing {selfDamage} damage to self!");
    }
}
