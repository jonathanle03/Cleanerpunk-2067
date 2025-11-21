using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Stagger")]
public class StaggerEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        if (target.staggerCD <= 0)
        {
            target.staggerCD = 2;
            target.moveable = false;
        }
        else
        {
            target.statusEffects.Remove(this);
        }
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
        target.moveable = true;
    }
}
