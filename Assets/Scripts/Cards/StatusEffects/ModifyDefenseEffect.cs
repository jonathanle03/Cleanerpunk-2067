using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Modify Defense")]
public class ModifyDefenseEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        target.defense = Mathf.CeilToInt(target.defense * effectPower);
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
        target.defense = Mathf.FloorToInt(target.defense / effectPower);
    }
}
