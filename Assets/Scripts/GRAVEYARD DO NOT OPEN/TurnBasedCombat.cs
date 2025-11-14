using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedCombat : MonoBehaviour
{
    // References for player and enemy objects
    [SerializeField] GameObject _player; 
    [SerializeField] GameObject _enemy;

    //Values for health values for player and enemy 
    [SerializeField] int _playerHealth = 11;
    [SerializeField] int _maxPlayerHealth = 11; 

    [SerializeField] int _enemyHealth = 10;
    [SerializeField] int _maxEnemyHealth = 10; 



    // UI element connections: player health, enemy health
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] TextMeshProUGUI enemyHealthText;
    [SerializeField] Button attackButton; 



    // Track which character's turn it is 
    private bool isPlayerTurn = true; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Show starting health values 
        UpdateUI();

        //player goes first
        isPlayerTurn = true;

        //set up attack button so it triggers player attack when clicked
        attackButton.onClick.AddListener(PlayerAttack); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayerAttack()
    {
        // Makes sure player can't attack out of turn 
        if(!isPlayerTurn)
        {
            return;
        }

      // If isPlayerTurn == true, lower enemy health
            _enemyHealth -= 2;

        // If the enemy dies in this attack (should be negative inclusive incase there's an overkill)...
            if (_enemyHealth <= 0)
            {
                //Reset the health to 0 (don't want to display a negative number) (unless?) 
                _enemyHealth = 0;
                // Update UI to reflect the hp change 
                UpdateUI();

                //Disable the attack button permanently 
                attackButton.interactable = false;
                Debug.Log("Enemy defeated!");
                return;
            }

            //If enemy doesn't die, disable player turn 
            isPlayerTurn = false;
            // Update UI to reflect hp changes
            UpdateUI();
            // Invoke is just calling a method with a slight time delay (method = EnemyAttack, time delay = 1f (1 second)) 
            Invoke(nameof(EnemyAttack), 1f);
    }

    void EnemyAttack()
    {
            
            // Lower player health
            _playerHealth -= 1; 


            // Same as in PlayerAttack- check if player is dead, then do the same things but for the player
            if (_playerHealth <= 0)
            {
                _playerHealth = 0;
                UpdateUI(); 
                attackButton.interactable = false;
                Debug.Log("Player defeated!");
                return; 
            }

            // If player isn't dead, return player control
            isPlayerTurn = true;
            UpdateUI(); 
    }

    
    // This method just contains everything needed to update the health values on the UI 
    void UpdateUI()
    {
        playerHealthText.text = _playerHealth + " / " + _maxPlayerHealth;
        enemyHealthText.text = _enemyHealth + " / " + _maxEnemyHealth; 
    }
}
