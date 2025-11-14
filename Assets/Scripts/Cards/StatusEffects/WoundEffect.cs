using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Wound")]
public class WoundEffect : StatusEffect
{
    public override void OnTurnEnd(Entity target)
    {
        if (target.isBoss)
        {
            target.currentHealth -= Mathf.CeilToInt(target.maxHealth * 0.03f);
        }
        else
        {
            target.currentHealth -= Mathf.CeilToInt(target.maxHealth * 0.1667f);
        }

        duration--;
        if (duration <= 0)
        {
            target.RemoveStatusEffect(this);
        }
    }
}
