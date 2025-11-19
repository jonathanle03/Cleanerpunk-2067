using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckScrollUI : MonoBehaviour
{
    [SerializeField] private RectTransform content;   // Content of the ScrollView
    [SerializeField] PlayerStats stats;
    public List<CardData> deck;  

    private void Start()
    {
        PopulateFromDeck();
    }

    private void PopulateFromDeck()
    {
        deck = new List<CardData>();
        deck = stats.getDeck();

    }

}
