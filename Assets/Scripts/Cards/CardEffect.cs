using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(Card card, Player player, Enemy target);
}
