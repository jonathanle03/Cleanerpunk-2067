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
    [SerializeField] private WinScreen winScreenData;
    [SerializeField]private Player playerDiscardStats;
    [SerializeField] private PlayerStats playerStats;
    
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

    private void Populate(String text)
    {
        ClearContent();
        if(text == "Deck"){
            deckList = playerStats.getDeck();
            foreach (CardData card in deckList)
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
        else if(text == "Discard"){
            discardList = playerDiscardStats.discards;
            foreach (CardData card in discardList)
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
        else
        {
            return;
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
        
        headerText.text = "";
        isOpen = false;
        deckPanel.SetActive(isOpen);

        deckList = new List<CardData>();
        deckList = playerStats.getDeck();

        discardList = new List<CardData>();
        discardList = playerDiscardStats.discards;
    }
    public void enablePanel(string text, List<CardData> source)
    {
        for(int i = 0; i < winScreenData.spareList.Length; i++)
        {
            winScreenData.spareButtons[i].gameObject.GetComponent<Button>().interactable = false;
            winScreenData.scrapButtons[i].gameObject.GetComponent<Button>().interactable = false;
        }
        for(int i = 0; i < 3; i++)
        {
            enemyManager.enemies[i].SetActive(false);
        }
        deckPanel.SetActive(true); 
        headerText.text = text;
        Populate(text);
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, (int) Math.Ceiling(deckList.Count / 3.0) * 425);
    }
    public void disablePanel()
    {
        for (int i = 0; i < winScreenData.spareButtons.Length; i++)
        {
            winScreenData.spareButtons[i].gameObject.GetComponent<Button>().interactable = true;
            winScreenData.scrapButtons[i].gameObject.GetComponent<Button>().interactable = true;
        }
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