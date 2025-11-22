using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PriceSet : MonoBehaviour
{

    [Header("Possible Shop Cards")]
    [SerializeField] List<CardData> attacks = new List<CardData>();
    [SerializeField] List<CardData> defenses = new List<CardData>();
    [SerializeField] List<CardData> supports = new List<CardData>();

    [Header("Health/Mana Upgrade Stuff")]
    [SerializeField] int HPprice;
    [SerializeField] int HPincrease;
    [SerializeField] int MPprice;
    [SerializeField] int MPincrease;
    

    [SerializeField] List<GameObject> cards = new List<GameObject>();

    [SerializeField] PlayerStats player;

    private int[] prices = new int[3];

    [Header("Text Attributes")]
    public TextMeshProUGUI balance; 
    public TextMeshProUGUI attackPrice;
    public TextMeshProUGUI defensePrice;
    public TextMeshProUGUI supportPrice;
    public TextMeshProUGUI HPbutton;
    public TextMeshProUGUI MPbutton;
    public TextMeshProUGUI HPtext;
    public TextMeshProUGUI MPtext; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        
        int randomChoice = Random.Range(0, attacks.Count);
        prices[0] = ((int)attacks[randomChoice].rarity + 1) * 50;
        attackPrice.text = "Attack price: " + prices[0];

        Debug.Log((int)attacks[randomChoice].rarity);
        Debug.Log(prices[0]);

        cards[0].GetComponent<cardReader>().card = attacks[randomChoice];
        cards[0].GetComponent<cardReader>().setCard();

        randomChoice = Random.Range(0, defenses.Count);
        prices[1] = ((int)defenses[randomChoice].rarity + 1) * 50;
        
        Debug.Log((int)attacks[randomChoice].rarity);
        Debug.Log(prices[1]);

        defensePrice.text = "Defense price: " + prices[1];
        cards[1].GetComponent<cardReader>().card = defenses[randomChoice];
        cards[1].GetComponent<cardReader>().setCard();

        randomChoice = Random.Range(0, supports.Count);
        prices[2] = ((int)supports[randomChoice].rarity + 1) * 50;

        Debug.Log((int)attacks[randomChoice].rarity);
        Debug.Log(prices[2]);

        supportPrice.text = "Support price: " + prices[2];
        cards[2].GetComponent<cardReader>().card = supports[randomChoice];
        cards[2].GetComponent<cardReader>().setCard();

        balance.text = "Balance: " + player.currency.ToString();


        HPtext.text = "+" + HPincrease + " to Health";
        MPtext.text = "+" + MPincrease + " to Energy"; 
        
        HPbutton.text = "Price: " + HPprice;
        MPbutton.text = "Price: " + MPprice; 

    }

    // Update is called once per frame
    void Update()
    {
        balance.text = "Balance: " + player.currency.ToString();
    }

    public void buyCard(int pick)
    {
        if(player.currency >= prices[pick])
        {
            Debug.Log("Purchasing: " + prices[pick]);
            player.currency -= prices[pick];
            player.playerDeck.Add(cards[pick].GetComponent<cardReader>().card);
        }


    }

    public void levelHP()
    {
        if(player.currency >= HPprice)
        {
            player.currency -= HPprice;
            player.maxHealth += HPincrease;
            Debug.Log("Upgraded health");
        }
    }

    public void levelMp()
    {
        if (player.maxMana < 9)
        {
            if (player.currency >= MPprice)
                    {
                        player.currency -= MPprice;
                        player.maxMana += MPincrease;
                        Debug.Log("Upgraded mana"); 
                    }
        }
        
    }

    public void toBattle()
    {
        if(player.maxMana < 9)
        {
            player.maxMana += 1;
        }
        
        SceneManager.LoadScene(1);
    }
}
