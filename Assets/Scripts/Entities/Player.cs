using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI energyText;

    [SerializeField] PlayerStats stats;
    [SerializeField] Enemy enemy;
    [SerializeField] List<CardData> deck = new List<CardData>();
    [SerializeField] List<CardData> discards = new List<CardData>();

    [SerializeField] public GameObject[] selectList = new GameObject[3];

    public const int maxMana = 9;
    public int currentMana;
    
    void Start()
    {
        currentMana = maxMana;

        deck = stats.getDeck();
    }
    private void Update()
    {
        energyText.text = "Energy: " + currentMana;
    }

    public void useSlash()
    {
        if(currentMana >= deck[0].cost)
        {
            
            currentMana -= deck[0].cost;
            if (deck[0].target == CardTarget.Enemy)
            {
                enemy.enemyButton.SetActive(true);
            }
        }
    }

    public void enemySelect(int pick)
    {
        deck[0].doEffect(this, selectList[pick].GetComponent<Enemy>());
        enemy.enemyButton.SetActive(false);
    }
}
