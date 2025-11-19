using UnityEngine;

[System.Serializable]
public class Card
{
    public CardData data { get; private set; }

    public int cost;
    public float power;
    public int hits;
    public int maxCooldown;

    public Card(CardData cardData)
    {
        data = cardData;
        cost = cardData.cost;
        power = cardData.power;
        maxCooldown = cardData.maxCooldown;
    }
}
