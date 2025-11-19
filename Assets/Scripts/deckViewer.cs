using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class deckViewer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button deckViewerButton;
    [SerializeField] PlayerStats stats;
    public List<CardData> deck;       


    [SerializeField] private GameObject deckViewerPanel;

    [SerializeField] private bool isOpen; 
    void Start()
    {
        deck = new List<CardData>();   
        isOpen = false;
        deck = stats.getDeck();
    }
    public void deckToggle()
    {
        if(!isOpen)
        {
            deckViewerPanel.SetActive(true);
            isOpen = true;
        }
        else
        {
            deckViewerPanel.SetActive(false);
            isOpen = false;
        }
    }
}
