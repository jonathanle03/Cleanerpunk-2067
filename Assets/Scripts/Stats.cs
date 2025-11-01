using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class Stats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] TextMeshProUGUI EnemyHealthText;
    [SerializeField] int playerHealth;

    [SerializeField] int cardDamage = 20;
    [SerializeField] GameObject passButton;
    [SerializeField] GameObject attackButton;
    private float timer = 0f;
    private bool doTimer = false;
    void Start()
    {
        playerHealthText.text = "Health: " + playerHealth;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f && doTimer)
        {
            returnButton();
        }
    }

    public void setEnemyHealth(int health)
    {
        EnemyHealthText.text = "Health: " + health.ToString();
    }

    public void playerDamage()
    {
        attackButton.SetActive(false);
        //Enemy1stats.enemy1Health -= cardDamage;
        //EnemyHealthText.text = "Health: " + enemyHealth;
        timer = 5f;
        doTimer = true;

    }

    public void enemyDamageonPass(int enemyDamage)
    {
        Debug.Log("before 5");
        passButton.SetActive(false);
        playerHealth -= enemyDamage;
        playerHealthText.text = ("Health: " + playerHealth);
        timer = 5f;
        doTimer = true;
    }

    private void returnButton()
    {
        Debug.Log("after 5");
        passButton.SetActive(true);
        attackButton.SetActive(true);
        doTimer = false;
    }
}
