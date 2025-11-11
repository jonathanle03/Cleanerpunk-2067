using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class BattleRemake : MonoBehaviour
{
    public EnemySpawn EnemySpawn;
    public Enemy1stats Enemy1stats;
    public Enemy_2_Stats Enemy2stats;
    [SerializeField] GameObject cardAttack;
    [SerializeField] TextMeshProUGUI textref;

    public int playerHealth = 200;
    public int cardDamage = 30;

    GameObject clickedButton;
    private int enemypos;

    public int energy = 4;
    [SerializeField] TextMeshProUGUI energyBar;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] GameObject passTurn;

    public float timer = 0f;
    private bool doTimer = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        energyBar.text = "Energy: " + energy;
        playerHealthText.text = "Health: " + playerHealth;
 
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (doTimer && timer <= 0)
        {
            buttonIntermission();
        }
    }

    public void playerAttack()
    {

        cardAttack.SetActive(false);
        for(int i = 0; i <= EnemySpawn.enemyCount; i++)
        {
            EnemySpawn.selectButtons[i].SetActive(true);
        }
    }

    public void attackSelect(int buttonIndex)
    {
        energy -= 1;
        energyBar.text = "Energy: " + energy;
        //Debug.Log(energy);
        enemypos = buttonIndex;
        //Debug.Log("button index: " + buttonIndex);
        int temp = EnemySpawn.healthBarsText[enemypos] -= cardDamage;
        EnemySpawn.healthTextComp[enemypos].text = "Health: " + temp;
        //Debug.Log(temp);
        //Debug.Log(enemypos);
        //textref.text = "Health: " + (temp);
        Debug.Log(energy);
        if(energy == 0)
        {
            for (int i = 0; i <= EnemySpawn.enemyCount; i++)
            {
                EnemySpawn.selectButtons[i].SetActive(false);
            }
            passTurn.SetActive(true);
        }

        if (temp == 0)
        {
            EnemySpawn.enemy1[enemypos].SetActive(false);
            EnemySpawn.enemy2[enemypos].SetActive(false);
            EnemySpawn.healthBars[enemypos].SetActive(false);
        }
    }

    public void passTurnSelect()
    {
        passTurn.SetActive(false);
        timer = EnemySpawn.enemyCount * 5;
        doTimer = true;
        for (int i = 0; i <= EnemySpawn.enemyCount; i++)
        {
            int typeOfAttack = EnemySpawn.enemychoice[i];
            if (typeOfAttack == 0)
            {
                playerHealth -= Enemy1stats.enemy1Damage;
                playerHealthText.text = "Health: " + playerHealth;
            }
            else if (typeOfAttack == 1)
            {
                playerHealth -= Enemy2stats.damage2;
                playerHealthText.text = "Health: " + playerHealth;
            }
        }

    }

    public void buttonIntermission()
    {
        cardAttack.SetActive(true);
        passTurn.SetActive(false);
        doTimer = false;
        energy = 4;
        energyBar.text = "Energy: " + energy;
    }
}
