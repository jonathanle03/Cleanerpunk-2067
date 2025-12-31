using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Modify Attack")]
public class ModifyAttackEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        target.attack = Mathf.CeilToInt(target.attack * effectPower);
    }

    public override void OnTurnEnd(Entity target)
    {
        duration--;
        if (duration <= 0)
        {
            target.RemoveStatusEffect(this);
        }
    }

    public override void OnExpire(Entity target)
    {
        target.attack = Mathf.FloorToInt(target.attack / effectPower);
    }
}
