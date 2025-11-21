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
            for (int i = 0; i < 3; i++)
            {
                if (enemyManager.enemies[i].GetComponent<Enemy>().isDead == true)
                {
                    check++;
                }
                if (check == 3)
                {
                    winup = true;
                    attackbutton.SetActive(false);
                    defenseButton.SetActive(false);
                    gameWin();
                    
                }
            }
        }

    }

    public void gameWin()
    {
        winScreenUI.SetActive(true);
        for(int i = 0; i < spareList.Length; i++)
        {
            spareList[i].GetComponent<cardReader>().card = (enemyManager.enemies[i].GetComponent<Enemy>().enemyCard);
            spareList[i].GetComponent<cardReader>().setCard();
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
