using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;
public class deckViewer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text headerText;    
    [SerializeField] private Transform content;
    [SerializeField] private GameObject cardItemPrefab;

    [SerializeField] private GameObject deckPanel;

    [Header("Data")]
    private PlayerStats playerStats;
    
    [SerializeField] public EnemyManager enemyManager;
    [SerializeField] public List<CardData> deckList;
    [SerializeField] public List<CardData> discardList;
    
    [SerializeField] private RectTransform contentRectTransform;

    private bool isOpen;
    [SerializeField] private bool isDiscardOpen;
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
        if(headerText.text == "Deck")
        {
            ClearContent();
            deckPanel.SetActive(false);
            headerText.text = "";
            return;
        }
        else if(headerText.text == "Discard") 
        {
            ClearContent();
            deckPanel.SetActive(true);
            deckList = new List<CardData>();
            deckList = playerStats.getDeck();
            headerText.text = "Deck";
            Populate(deckList);
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, (int) Math.Ceiling(deckList.Count / 3.0) * 425);
            return;
        }    
        else{
            deckPanel.SetActive(true);
            deckList = new List<CardData>();
            deckList = playerStats.getDeck();
            headerText.text = "Deck";
            Populate(deckList);
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, (int) Math.Ceiling(deckList.Count / 3.0) * 425);
        }   
    }
    public void ShowDiscard()
    {
        if(headerText.text == "Discard")
        {
            deckPanel.SetActive(false);
            ClearContent();
            headerText.text = "";
            return;
        } 
        else if(headerText.text == "Deck")
        {
            deckPanel.SetActive(true);
            ClearContent();
            discardList = new List<CardData>();
            discardList = playerStats.getDeck();
            headerText.text = "Discard";
            Populate(discardList);
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, (int) Math.Ceiling(deckList.Count / 3.0) * 425);
            return;
        }
        else {
            deckPanel.SetActive(true);
            discardList = new List<CardData>();
            discardList = playerStats.getDeck();  
            headerText.text = "Discard";
            Populate(discardList);
            contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, (int) Math.Ceiling(deckList.Count / 3.0) * 425);
        }
    }
    public void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

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
            for(int i = 0; i < 3; i++)
            {
                enemyManager.enemies[i].SetActive(false);
            }
            deckPanel.SetActive(true);
            isOpen = true;
        }
        else 
        {
            for(int i = 0; i < 3; i++)
            {
                if(enemyManager.enemies[i].GetComponent<Enemy>().isDead == false)
                {
                    enemyManager.enemies[i].SetActive(true);   
                }
            }
            deckPanel.SetActive(false);
            isOpen = false;
        }
    }
}