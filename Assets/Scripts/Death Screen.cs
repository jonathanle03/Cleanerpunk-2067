using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] public EnemyManager enemyManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            enemyManager.enemies[i].SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deathButton()
    {
        SceneManager.LoadScene(0);
    }
}
