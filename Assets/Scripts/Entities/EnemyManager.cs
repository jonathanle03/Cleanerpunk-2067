using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public GameObject[] enemies = new GameObject[3];
    [SerializeField] public GameObject[] enemyButtons = new GameObject[3];

    [SerializeField] Player player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

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

        if (player.playerTurn == false)
        {
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
                        enemy.enemyCard.doEffect(enemy, player);
                        enemies[i].GetComponent<Enemy>().RandomCard();
                    }

                    for (int j = enemy.statusEffects.Count - 1; j >= 0; --j)
                    {
                        enemy.statusEffects[j].OnTurnEnd(enemy);
                    }
                }
            }

            foreach(StatusEffect effect in player.statusEffects)
            {
                effect.OnTurnStart(player);
            }

            if (player.moveable == true)
            {
                player.playerTurn = true;
            }
            else
            {
                player.fillHand();
            }
        }
    }

    void EnemyDied()
    {

    }
}
