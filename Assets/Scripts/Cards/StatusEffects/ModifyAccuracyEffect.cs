using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Modify Accuracy")]
public class ModifyAccuracyEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        target.accuracy = Mathf.CeilToInt(target.accuracy * effectPower);
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
        target.accuracy = Mathf.FloorToInt(target.accuracy / effectPower);
    }
}
