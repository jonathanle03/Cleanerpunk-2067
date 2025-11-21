using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;
    [SerializeField] public List<CardData> playerDeck = new List<CardData>();

    public int maxHealth;
    public int attack;
    public int defense;
    public int maxMana;
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
