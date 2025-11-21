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
    [SerializeField]private Player playerDiscardStats;
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
            disablePanel();
            return;
        }
        else if(headerText.text == "Discard") 
        {
            ClearContent();
            enablePanel("Deck", deckList);
            return;
        }    
        else{
            enablePanel("Deck", deckList);
        }   
    }
    public void ShowDiscard()
    {
        if(headerText.text == "Discard")
        {
            disablePanel();
            return;
        } 
        else if(headerText.text == "Deck")
        {
            ClearContent();
            enablePanel("Discard", discardList);;
            return;
        }
        else {
           enablePanel("Discard", discardList);
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
        discardList = playerDiscardStats.discards;
    }
    public void enablePanel(string text, List<CardData> source)
    {
        for(int i = 0; i < 3; i++)
        {
            enemyManager.enemies[i].SetActive(false);
        }
        deckPanel.SetActive(true); 
        headerText.text = text;
        Populate(source);
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, (int) Math.Ceiling(deckList.Count / 3.0) * 425);
    }
    public void disablePanel()
    {
        for(int i = 0; i < 3; i++)
        {
            if(enemyManager.enemies[i].GetComponent<Enemy>().isDead == false)
            {
                enemyManager.enemies[i].SetActive(true);   
            }
        }
        deckPanel.SetActive(false);
        ClearContent();
        headerText.text = "";
            
    }
}