using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;
    [SerializeField] public List<CardData> playerDeck = new List<CardData>();

    public int maxHealth;
    public int attack;
    public int defense;
    public int force;
    public int shield;
    public int maxMana;
    public int currency;
    public int accuracy;
    public int evasion;

    [Header("Damage Resistances")]
    public DamageResistances damageResistances = new DamageResistances();
    
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc down");
            Application.Quit(); 
        }
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
