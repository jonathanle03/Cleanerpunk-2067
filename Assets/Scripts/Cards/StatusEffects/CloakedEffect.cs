using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Cloaked")]
public class CloakedEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        if (target is Player)
        {
            Player player = (Player) target;
            player.isInvisible = true;
        }
    }

    public override void OnTurnEnd(Entity target)
    {
        if (target is Player)
        {
            Player player = (Player) target;
            if (player.currentMana < 3)
            {
                player.isInvisible = false;
                player.RemoveStatusEffect(this);
            }
            else
            {
                player.currentMana -= 3;
            }
        }
        else
        {
            target.RemoveStatusEffect(this);
        }
    }

    public override float ModifyIncomingDamage(float damage)
    {
        return 0f;
    }
}
