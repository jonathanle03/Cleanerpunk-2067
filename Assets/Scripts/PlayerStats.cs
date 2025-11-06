using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
     private static PlayerStats instance;
     //[SerializeField] CardData[] card = new CardData[20];
     [SerializeField] List<CardData> playerDeck = new List<CardData>();       
        
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
       // playerDeck.Add(card[0]);
        
        Debug.Log("" + playerDeck.ToString());
    }
    public List<CardData> getDeck()
    {
        return playerDeck;
    }
}
