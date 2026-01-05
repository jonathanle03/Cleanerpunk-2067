using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Setup")]
    [SerializeField] public GameObject[] enemies = new GameObject[3];
    [SerializeField] public GameObject[] enemyButtons = new GameObject[3];
    [SerializeField] List<EnemyData> possibleEnemies = new List<EnemyData>();

    [Header("Enemy Count Settings")]
    [Tooltip("If true, uses a random count (min-max). If false, uses assigned enemy count.")]
    [SerializeField] private bool useRandomEnemyCount = false;
    [Tooltip("Minimum enemies when using random count (1-3)")]
    [SerializeField] [Range(1, 3)] private int minEnemyCount = 1;
    [Tooltip("Maximum enemies when using random count (1-3)")]
    [SerializeField] [Range(1, 3)] private int maxEnemyCount = 3;

    [SerializeField] Player player;

    [SerializeField] List<string> textLog = new List<string>();
    [SerializeField] TextMeshProUGUI textLogText;

    [SerializeField] private float timing;

    private bool _isRunning = false;
    private int _activeEnemyCount = 0;

    public int ActiveEnemyCount => _activeEnemyCount;

    void Start()
    {
        InitializeEnemies();
    }

    private void InitializeEnemies()
    {
        // Validate that we have at least one enemy assigned
        int assignedCount = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                assignedCount++;
            }
        }

        if (assignedCount == 0)
        {
            Debug.LogError("EnemyManager: No enemies assigned! At least one enemy must be assigned.");
            return;
        }

        // Determine how many enemies to activate
        if (useRandomEnemyCount)
        {
            // Ensure min <= max
            int validMin = Mathf.Clamp(minEnemyCount, 1, 3);
            int validMax = Mathf.Clamp(maxEnemyCount, validMin, 3);
            _activeEnemyCount = Random.Range(validMin, validMax + 1);
            _activeEnemyCount = Mathf.Min(_activeEnemyCount, assignedCount); // Can't exceed assigned count
        }
        else
        {
            // Use all assigned enemies
            _activeEnemyCount = assignedCount;
        }

        // Initialize active enemies and deactivate excess
        int activeCount = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                if (activeCount < _activeEnemyCount)
                {
                    // Initialize this enemy
                    Enemy enemy = enemies[i].GetComponent<Enemy>();
                    EnemyData enemyData = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
                    enemy.InitializeEnemy(enemyData);
                    enemy.enemyButton = enemyButtons[i];
                    activeCount++;
                }
                else
                {
                    // Deactivate excess enemy
                    enemies[i].SetActive(false);
                    if (enemyButtons[i] != null)
                    {
                        enemyButtons[i].SetActive(false);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && enemies[i].activeSelf && enemies[i].GetComponent<Enemy>().currentHealth <= 0)
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
            if (enemies[i] == null || !enemies[i].activeSelf) continue;

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
