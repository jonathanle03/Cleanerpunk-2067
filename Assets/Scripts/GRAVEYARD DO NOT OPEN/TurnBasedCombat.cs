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
        if(!isPlayerTurn)
        {
            return;
        }

      
            _enemyHealth -= 2;

            if (_enemyHealth <= 0)
            {
                _enemyHealth = 0;
                UpdateUI();
                attackButton.interactable = false;
                Debug.Log("Enemy defeated!");
                return;
            }

            isPlayerTurn = false;
            UpdateUI();
            Invoke(nameof(EnemyAttack), 1f);
    }

    void EnemyAttack()
    {
 
            _playerHealth -= 1; 

            if (_playerHealth <= 0)
            {
                _playerHealth = 0;
                UpdateUI(); 
                attackButton.interactable = false;
                Debug.Log("Player defeated!");
                return; 
            }

            isPlayerTurn = true;
            UpdateUI(); 
    }


    void UpdateUI()
    {
        playerHealthText.text = _playerHealth + " / " + _maxPlayerHealth;
        enemyHealthText.text = _enemyHealth + " / " + _maxEnemyHealth; 
    }
}
