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

    [Header("Stat Upgrade Stuff")]
    [SerializeField] int ATKprice = 25;
    [SerializeField] int ATKincrease = 5;
    [SerializeField] int DEFprice = 25;
    [SerializeField] int DEFincrease = 5;
    [SerializeField] int FRCprice = 25;
    [SerializeField] int FRCincrease = 5;
    [SerializeField] int SHDprice = 25;
    [SerializeField] int SHDincrease = 5;
    [SerializeField] int ACCprice = 25;
    [SerializeField] int ACCincrease = 1;
    [SerializeField] int EVAprice = 25;
    [SerializeField] int EVAincrease = 1;

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

    [Header("Stat Upgrade UI")]
    public TextMeshProUGUI ATKbutton;
    public TextMeshProUGUI DEFbutton;
    public TextMeshProUGUI FRCbutton;
    public TextMeshProUGUI SHDbutton;
    public TextMeshProUGUI ACCbutton;
    public TextMeshProUGUI EVAbutton;
    public TextMeshProUGUI ATKtext;
    public TextMeshProUGUI DEFtext;
    public TextMeshProUGUI FRCtext;
    public TextMeshProUGUI SHDtext;
    public TextMeshProUGUI ACCtext;
    public TextMeshProUGUI EVAtext; 

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


        HPtext.text = "+" + HPincrease + " HP";
        MPtext.text = "+" + MPincrease + " MP";

        HPbutton.text = "Price: " + HPprice;
        MPbutton.text = "Price: " + MPprice;

        // Initialize stat upgrade UI
        if (ATKtext != null) ATKtext.text = "ATK: " + player.attack + " (+" + ATKincrease + ")";
        if (DEFtext != null) DEFtext.text = "DEF: " + player.defense + " (+" + DEFincrease + ")";
        if (FRCtext != null) FRCtext.text = "FRC: " + player.force + " (+" + FRCincrease + ")";
        if (SHDtext != null) SHDtext.text = "SHD: " + player.shield + " (+" + SHDincrease + ")";
        if (ACCtext != null) ACCtext.text = "ACC: " + player.accuracy + " (+" + ACCincrease + ")";
        if (EVAtext != null) EVAtext.text = "EVA: " + player.evasion + " (+" + EVAincrease + ")";

        if (ATKbutton != null) ATKbutton.text = "Price: " + ATKprice;
        if (DEFbutton != null) DEFbutton.text = "Price: " + DEFprice;
        if (FRCbutton != null) FRCbutton.text = "Price: " + FRCprice;
        if (SHDbutton != null) SHDbutton.text = "Price: " + SHDprice;
        if (ACCbutton != null) ACCbutton.text = "Price: " + ACCprice;
        if (EVAbutton != null) EVAbutton.text = "Price: " + EVAprice;

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

    public void levelATK()
    {
        if (player.currency >= ATKprice)
        {
            player.currency -= ATKprice;
            player.attack += ATKincrease;
            if (ATKtext != null) ATKtext.text = "ATK: " + player.attack + " (+" + ATKincrease + ")";
            Debug.Log("Upgraded ATK to " + player.attack);
        }
    }

    public void levelDEF()
    {
        if (player.currency >= DEFprice)
        {
            player.currency -= DEFprice;
            player.defense += DEFincrease;
            if (DEFtext != null) DEFtext.text = "DEF: " + player.defense + " (+" + DEFincrease + ")";
            Debug.Log("Upgraded DEF to " + player.defense);
        }
    }

    public void levelFRC()
    {
        if (player.currency >= FRCprice)
        {
            player.currency -= FRCprice;
            player.force += FRCincrease;
            if (FRCtext != null) FRCtext.text = "FRC: " + player.force + " (+" + FRCincrease + ")";
            Debug.Log("Upgraded FRC to " + player.force);
        }
    }

    public void levelSHD()
    {
        if (player.currency >= SHDprice)
        {
            player.currency -= SHDprice;
            player.shield += SHDincrease;
            if (SHDtext != null) SHDtext.text = "SHD: " + player.shield + " (+" + SHDincrease + ")";
            Debug.Log("Upgraded SHD to " + player.shield);
        }
    }

    public void levelACC()
    {
        if (player.currency >= ACCprice)
        {
            player.currency -= ACCprice;
            player.accuracy += ACCincrease;
            if (ACCtext != null) ACCtext.text = "ACC: " + player.accuracy + " (+" + ACCincrease + ")";
            Debug.Log("Upgraded ACC to " + player.accuracy);
        }
    }

    public void levelEVA()
    {
        if (player.currency >= EVAprice)
        {
            player.currency -= EVAprice;
            player.evasion += EVAincrease;
            if (EVAtext != null) EVAtext.text = "EVA: " + player.evasion + " (+" + EVAincrease + ")";
            Debug.Log("Upgraded EVA to " + player.evasion);
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
    
    public void toBoss()
    {
        if(player.maxMana < 9)
        {
            player.maxMana += 1;
        }

        SceneManager.LoadScene(3);
    }
}
