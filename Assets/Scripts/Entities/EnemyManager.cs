using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] public GameObject[] enemies = new GameObject[3];
    [SerializeField] public GameObject[] enemyButtons = new GameObject[3];
    [SerializeField] List<EnemyData> possibleEnemies = new List<EnemyData>();

    [SerializeField] Player player;

    [SerializeField] List<string> textLog = new List<string>();
    [SerializeField] TextMeshProUGUI textLogText;

    [SerializeField] private float timing; 

    private bool _isRunning = false; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            EnemyData enemyData = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
            enemy.InitializeEnemy(enemyData);
            enemy.enemyButton = enemyButtons[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].GetComponent<Enemy>().currentHealth <= 0)
            {
                enemies[i].SetActive(false);
                enemies[i].GetComponent<Enemy>().isDead = true;
            }
        }

        if (!player.playerTurn && !_isRunning)
        {
            StartCoroutine(EnemyTurn()); 
        }
    }


    IEnumerator EnemyTurn()
    {
        _isRunning = true;

        // Disable player cards at the start of enemy turn
        player.DisableCards(); 
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            if (!enemy.isDead)
            {
                foreach (StatusEffect effect in enemy.statusEffects)
                {
                    effect.OnTurnStart(enemy);
                }

                if (enemy.moveable)
                {
                    string difference = "";
                    if (textLog.Count == 3)
                    {
                        textLog.RemoveAt(0);

                    }
                    textLog.Add(enemy.entityName + ": " + enemy.enemyCard.name);
                    for (int k = 0; k < textLog.Count; k++)
                    {
                        difference += ("\n" + textLog[k]);
                    }
                    textLogText.text = difference;

                    // Show card name popup above enemy
                    enemy.ShowCardPopup();

                    yield return StartCoroutine(enemy.enemyCard.doEffectWithDelay(enemy, player));

                    yield return new WaitForSeconds(timing);


                    enemies[i].GetComponent<Enemy>().SelectNextCard();
                }

                for (int j = enemy.statusEffects.Count - 1; j >= 0; --j)
                {
                    enemy.statusEffects[j].OnTurnEnd(enemy);
                    enemy.uiController.UpdateStatusUI(enemy.statusEffects);
                }

                if (enemy.staggerCD > 0)
                {
                    enemy.staggerCD--;
                }
            }
        }

        foreach (StatusEffect effect in player.statusEffects)
        {
            effect.OnTurnStart(player);
        }

        // Re-enable player cards at the end of enemy turn
        player.EnableCards();

        if (player.moveable == true)
        {
            player.playerTurn = true;
        }
        else
        {
            player.fillHand();
        }
        _isRunning = false;
    }
}
