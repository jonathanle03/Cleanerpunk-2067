using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    private bool playerTurn;

    public playerStats player;
    public enemyStats enemy;

    int activeBlock;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] Button attackButton;
    [SerializeField] Button blockButton; 

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        GameObject enemyObject = GameObject.Find("Enemy");

        player = playerObject.GetComponent<playerStats>(); 
        enemy = enemyObject.GetComponent<enemyStats>();

        attackButton.onClick.AddListener(playerAttack);
        blockButton.onClick.AddListener(playerBlock);

        playerTurn = true; 

        UpdateUI(); 
    }

    void playerAttack()
    {
        // Makes sure player can't attack out of turn 
        if (!playerTurn)
        {
            return;
        }

        enemy.damageHealth(player.getAttack());

        if(enemy.getHealth() <= 0)
        {
            enemy.setHealth(0);

            endBattle(false); 
        }

        playerTurn = false;

        Invoke(nameof(enemyAttack), 1f); 

    }

    void playerBlock()
    {
        // Makes sure player can't block out of turn 
        if (!playerTurn)
        {
            return;
        }

        player.addBlock();

        playerTurn = false;

        Invoke(nameof(enemyAttack), 1f); 


    }

    void enemyAttack()
    {
        player.damageHealth(enemy.getAttack());

        if (player.getHealth() <= 0)
        {
            player.setHealth(0);

            endBattle(true); 
        }

        playerTurn = true;

        UpdateUI(); 


    }

    void UpdateUI()
    {
        playerText.text = player.getBlock() + " " + player.getHealth() + " / " + player.getMaxHealth();
        enemyText.text = enemy.getHealth() + " / " + enemy.getMaxHealth(); 
    }

    void endBattle(bool isPlayerDead)
    {
        UpdateUI();

        attackButton.interactable = false;
        blockButton.interactable = false;

        if(isPlayerDead)
        {
            Debug.Log("game over!!!!");
        } else
        {
            Debug.Log("you win!!!!"); 
        }

        return;
    }
}
