using System;
using TMPro;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Enemy1stats Enemy1Stats;
    public Enemy_2_Stats Enemy_2_Stats;

    public GameObject[] enemy1 = new GameObject[3];
    public GameObject[] enemy2 = new GameObject[3];
    public GameObject[] selectButtons = new GameObject[3];
    public int[] healthBarsText = new int[3];


    public GameObject[] healthBars = new GameObject[3];
    public TextMeshProUGUI[] healthTextComp = new TextMeshProUGUI[3];

    public int enemyCount;
    public int[] enemychoice = new int[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyCount = UnityEngine.Random.Range(0, 3);
        for (int i = 0; i <= enemyCount; i++)
        {
            enemychoice[i] = UnityEngine.Random.Range(0, 2);
            setEnemy(enemychoice[i], i);
        }
        Debug.Log(healthBarsText[1]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setEnemy(int enemy, int position)
    {
        if(enemy == 0)
        {
            enemy1[position].SetActive(true);
            healthBars[position].SetActive(true);
            healthTextComp[position].text = "Health: " + Enemy1Stats.enemy1Health;
            healthBarsText[position] = Enemy1Stats.enemy1Health;
        } else if(enemy == 1)
        {
            enemy2[position].SetActive(true);
            healthBars[position].SetActive(true);
            healthTextComp[position].text = "Health: " + Enemy_2_Stats.health2;
            healthBarsText[position] = Enemy_2_Stats.health2;
        }
    }
}
