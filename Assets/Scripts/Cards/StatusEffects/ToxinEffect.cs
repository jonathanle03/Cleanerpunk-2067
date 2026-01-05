using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Toxin")]
public class ToxinEffect : StatusEffect
{
    public override void OnTurnEnd(Entity target)
    {
        if (target.isBoss)
        {
            target.currentHealth -= Mathf.CeilToInt(target.currentHealth * 0.02f);
        }
        else
        {
            target.currentHealth -= Mathf.CeilToInt(target.currentHealth * 0.1f);
        }

        duration--;
        if (duration <= 0)
        {
            target.RemoveStatusEffect(this);
        }
    }
}
