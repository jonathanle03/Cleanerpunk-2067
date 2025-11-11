using UnityEngine;

[System.Serializable]
public class Card
{
    public CardData data { get; private set; }

    public int cost;
    public int damage;
    public int hits;

    public Card(CardData cardData)
    {
        data = cardData;
        cost = cardData.cost;
        damage = cardData.damage;
    }
}
