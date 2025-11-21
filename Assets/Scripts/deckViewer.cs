using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class deckViewer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text headerText;    
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardItemPrefab;

    [SerializeField] private GameObject deckPanel;

    [Header("Data")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] public List<CardData> deckList;
    [SerializeField] public List<CardData> discardList;

    private bool isOpen;
    private void ClearContent()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);
    }

    private void Populate(List<CardData> source)
    {
        ClearContent();

        foreach (CardData card in source)
        {
            GameObject obj = Instantiate(cardItemPrefab, content, false);

            cardReader ui = obj.GetComponent<cardReader>();
            ui.card = card;
            if (ui != null)
            {
                ui.card = card;
                ui.setCard();
            }
        }
    }

    public void ShowDeck()
    {
        headerText.text = "Deck";
        Populate(deckList);
    }

    public void ShowDiscard()
    {
        headerText.text = "Discard";
        Populate(discardList);
    }
    public void Start()
    {
        isOpen = false;
        deckPanel.SetActive(isOpen);

        deckList = new List<CardData>();
        deckList = playerStats.getDeck();

        discardList = new List<CardData>();
        discardList = playerStats.getDeck();        
    }
    public void ToggleDeckPanel()
    {
        if(!isOpen)
        {
            deckPanel.SetActive(true);
            isOpen = true;
        }
        else
        {
            deckPanel.SetActive(false);
            isOpen = false;
        }
    }
}