using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(CardData card, Player player, Enemy enemy);
}
