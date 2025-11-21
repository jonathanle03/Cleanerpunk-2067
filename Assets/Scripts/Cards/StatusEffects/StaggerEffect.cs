using UnityEngine;
using UnityEngine.Rendering.UI;

[CreateAssetMenu(menuName = "Status Effects/Stagger")]
public class StaggerEffect : StatusEffect
{
    public override void ApplyEffect(Entity target)
    {
        target.moveable = false;
        Debug.Log("stagger");
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
