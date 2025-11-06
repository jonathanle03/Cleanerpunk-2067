using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public CardTarget target;
    public abstract void Execute(CardData card, Player player, Enemy enemy, CardTarget target);
}
