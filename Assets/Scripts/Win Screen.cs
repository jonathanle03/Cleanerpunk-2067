using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject attackbutton;
    [SerializeField] GameObject defenseButton;

    [SerializeField] public GameObject winScreenUI;
    [SerializeField] public EnemyManager enemyManager;
    public PlayerStats playerStats;

    [SerializeField] public GameObject[] spareList = new GameObject[3];
    [SerializeField] public GameObject[] spareButtons = new GameObject[3];
    [SerializeField] public GameObject[] scrapButtons = new GameObject[3];

    [SerializeField] TextMeshProUGUI scrapCounter;

    private int deadCount;
    private bool winup = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        playerStats = GameObject.Find("playerManager").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        scrapCounter.text = "Current Scrap: " + playerStats.currency;
        if (winup == false) {
            int check = 0;
            int activeEnemies = 0;

            // Count active and dead enemies
            for (int i = 0; i < 3; i++)
            {
                if (enemyManager.enemies[i] != null && enemyManager.enemies[i].activeSelf)
                {
                    activeEnemies++;
                    if (enemyManager.enemies[i].GetComponent<Enemy>().isDead == true)
                    {
                        check++;
                    }
                }
            }

            // Win when all active enemies are dead
            if (activeEnemies > 0 && check == activeEnemies)
            {
                winup = true;
                attackbutton.SetActive(false);
                defenseButton.SetActive(false);
                gameWin();
            }
        }

    }

    public void gameWin()
    {
        winScreenUI.SetActive(true);

        // Display loot for enemies that were defeated (dead enemies)
        for(int i = 0; i < spareList.Length; i++)
        {
            if (enemyManager.enemies[i] != null)
            {
                Enemy enemy = enemyManager.enemies[i].GetComponent<Enemy>();
                // Check if enemy was in battle (dead enemies that dropped loot)
                if (enemy != null && enemy.isDead)
                {
                    spareList[i].GetComponent<cardReader>().card = (enemy.enemyData.cards[0]);
                    spareList[i].GetComponent<cardReader>().setCard();
                }
                else
                {
                    // Hide buttons for enemy slots that were never used
                    if (spareButtons[i] != null)
                    {
                        spareButtons[i].SetActive(false);
                    }
                    if (scrapButtons[i] != null)
                    {
                        scrapButtons[i].SetActive(false);
                    }
                }
            }
            else
            {
                // Hide buttons for null enemy slots
                if (spareButtons[i] != null)
                {
                    spareButtons[i].SetActive(false);
                }
                if (scrapButtons[i] != null)
                {
                    scrapButtons[i].SetActive(false);
                }
            }
        }
    }

    public void spare(int pick)
    {
        playerStats.playerDeck.Add(spareList[pick].GetComponent<cardReader>().card);
        spareButtons[pick].SetActive(false);
        scrapButtons[pick].SetActive(false);

    }

    public void scrap(int pick)
    {
        playerStats.currency += 50;
        spareButtons[pick].SetActive(false);
        scrapButtons[pick].SetActive(false);
    }

    public void toShop()
    {
        SceneManager.LoadScene(2);
    }
}
