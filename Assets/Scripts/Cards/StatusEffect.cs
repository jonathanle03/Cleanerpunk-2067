using UnityEngine;

public class StatusEffect : ScriptableObject
{
    public string effectName;
    public int duration;
    public float applicationChance;

    public virtual void ApplyEffect(Entity target) { }
    public virtual void OnTurnStart(Entity target) { }
    public virtual void OnTurnEnd(Entity target) { }
    public virtual void OnExpire(Entity target) { }

    public virtual float ModifyIncomingDamage(float damage) => damage;
    public virtual float ModifyOutgoingDamage(float damage) => damage;
}
