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
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null && enemies[i].GetComponent<Enemy>().health <= 0)
            {
                enemies[i].SetActive(false);
                enemies[i].GetComponent<Enemy>().isDead = true;
            }
        }

        if(player.playerTurn == false) 
        {
            for(int i = 0; i < enemies.Length; i++)
            {
                if(enemies[i].GetComponent<Enemy>().isDead == false)
                {
                    player.takeDamage(enemies[i].GetComponent<Enemy>().enemyCard.damage, enemies[i].GetComponent<Enemy>().enemyAttack);
                }
                
            }
            
            player.playerTurn = true;
        }
    }

    void EnemyDied()
    {

    } 
}
