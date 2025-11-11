using UnityEngine;

public class Card : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string cardName;
    public int cost;
    public int numOwned;

    public void Start()
    {
        numOwned = 0;
    }


    public void incrementNumOwned()
    {
        ++numOwned;
    }
}
