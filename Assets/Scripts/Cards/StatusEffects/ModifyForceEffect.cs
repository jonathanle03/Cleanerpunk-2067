using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Modify Force")]
public class ModifyForceEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        target.force = Mathf.CeilToInt(target.force * effectPower);
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
        target.force = Mathf.FloorToInt(target.force / effectPower);
    }
}
