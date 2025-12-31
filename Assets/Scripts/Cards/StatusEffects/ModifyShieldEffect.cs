using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Modify Shield")]
public class ModifyShieldEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        target.shield = Mathf.CeilToInt(target.shield * effectPower);
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
        target.shield = Mathf.FloorToInt(target.shield / effectPower);
    }
}
