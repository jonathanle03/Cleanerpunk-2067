using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Modify Evasion")]
public class ModifyEvasionEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        target.evasion = Mathf.CeilToInt(target.evasion * effectPower);
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
        target.evasion = Mathf.FloorToInt(target.evasion / effectPower);
    }
}
