using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // Tracking which character's turn it is 
    private bool playerTurn;

    // Initializations for the classes used to store info, they're null here
    // We use the methods from these a LOT. They handle the stats for player and enemy respectively. 
    // Anytime you see in the code "player.doSomething()" that's a method that belongs to those scripts. 
    public playerStats player;
    public enemyStats enemy;

    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI enemyText;
    [SerializeField] Button attackButton;
    [SerializeField] Button blockButton;

    private IEnumerator coroutine; 

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // First finding the player and enemy objects
        // They're intialized here because we don't need the game objects later, just the stat script components. 
        GameObject playerObject = GameObject.Find("Player");
        GameObject enemyObject = GameObject.Find("Enemy");

        // Finding the stats component of the player and enemy respectively 
        player = playerObject.GetComponent<playerStats>(); 
        enemy = enemyObject.GetComponent<enemyStats>();

        // Activating the buttons, and adding the method they will use when clicked
        attackButton.onClick.AddListener(playerAttack);
        blockButton.onClick.AddListener(playerBlock);

        //Player goes first
        playerTurn = true; 

        // Update UI with player and enemy health 
        Invoke(nameof(UpdateUI), 0.25f); 
    }

    
    void playerAttack()
    {
        // Makes sure player can't attack out of turn 
        if (!playerTurn)
        {
            return;
        }

        // Calling the enemy damage method, with the attack stat of the player
        enemy.damageHealth(player.getAttack());

        // If the enemy is dead (below or at 0 health), reset health to zero and use the endBattle method. 
        if(enemy.getHealth() <= 0)
        {
            enemy.setHealth(0);

            endBattle(false); 
        }

        // No longer the player's turn.
        playerTurn = false;

        coroutine = timedEnemyAttack();
        StartCoroutine(coroutine);

    }


    void playerBlock()
    {
        // Makes sure player can't block out of turn 
        if (!playerTurn)
        {
            return;
        }


        // Calls the player's add block method 
        player.addBlock();

        playerTurn = false;

        coroutine = timedEnemyAttack();
        StartCoroutine(coroutine);


    }

    IEnumerator timedEnemyAttack()
    {
        enemyAttack();

        yield return new WaitForSeconds(1);
        UpdateUI();
        playerTurn = true; 
    }
    
    // Enemy action. Very similar to the PlayerAttack method.
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

    // This method just updates the health text- should be used whenever a change occurs. 
    void UpdateUI()
    {
        // Tried to get colored text here for the block but I couldnt get it to work lol :-( 
        playerText.text = player.getBlock() + " " + player.getHealth() + " / " + player.getMaxHealth();
        enemyText.text = enemy.getHealth() + " / " + enemy.getMaxHealth(); 
    }

    // Method for when the battle ends (so it's not repeated in both player and enemy attack like in the og script). 
    void endBattle(bool isPlayerDead)
    {
        UpdateUI();

        // Disable player interaction 
        attackButton.interactable = false;
        blockButton.interactable = false;

        // U may have noticed when I called endBattle earlier I called it with a true/false. This is just checking if the player died or not. This will matter when we
        // actually implement a game over state. 
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
