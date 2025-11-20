using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
     private static PlayerStats instance;
     //[SerializeField] CardData[] card = new CardData[20];
     [SerializeField] public List<CardData> playerDeck = new List<CardData>();

     public int maxHealth;
     public int defense;
     public int maxEnergy;
     public int attack;
     public int currency;
        
        void Awake()
        {
            // this makes sure there's only one game manager
            if (instance != null && instance != this)
           {
               Destroy(gameObject);
               return;
           }

           instance = this;
           DontDestroyOnLoad(gameObject); // <- now it would persist across scenes
       }

    private void Start()
    {
        maxHealth = 100;
        defense = 10;
        maxEnergy = 4;
        attack = 10;
       // playerDeck.Add(card[0]);
        
        Debug.Log("" + playerDeck.ToString());
    }
    public List<CardData> getDeck()
    {
        List<CardData> newList = new List<CardData>();
        foreach (CardData c in playerDeck)
        {
            newList.Add(c);
        }
        return newList;
    }
}
