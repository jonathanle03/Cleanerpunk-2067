using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections; 

public class Enemy : Entity
{
    [Header("Enemy Specific Info")]
    [SerializeField] public EnemyData enemyData;
    public Sprite art;
    public CardData enemyCard;
    [SerializeField] public GameObject enemyButton;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float _timing;

    public bool isDead;
    private Coroutine healthBarCoroutine;

    private void Start()
    {
        
    }

    private void Update()
    {
        healthText.text = "" + currentHealth;
    }

    public override void EntityDead()
    {
        isDead = true;
        enemyButton.SetActive(false);
    }

    public void InitializeEnemy(EnemyData data)
    {
        enemyData = data;
        entityName = enemyData.enemyName;
        art = enemyData.art;
        maxHealth = enemyData.maxHealth;
        attack = enemyData.attack;
        defense = enemyData.defense;
        isBoss = enemyData.isBoss;
        deck = new List<CardData>(enemyData.cards);

        SpriteRenderer thisrender = GetComponent<SpriteRenderer>();
        thisrender.sprite = art; 

        currentHealth = maxHealth;
        healthBarMaxWidth = (int)healthBar.GetComponent<Transform>().localScale.x;
        healthBarStartPos = healthBar.GetComponent<Transform>().localPosition;
        healthText.text = "Health: " + currentHealth;
        RandomCard();
    }

    public void RandomCard()
    {
        enemyCard = deck[Random.Range(0, deck.Count)];
    }


    public override void TakeDamage(float cardPower, int incomingAttack)
    {
        // Prevent dead enemies from taking damage
        if (isDead)
        {
            return;
        }

        float incomingDmg = cardPower * incomingAttack * 100f / (100f + defense);
        foreach (StatusEffect effect in statusEffects)
        {
            incomingDmg = effect.ModifyIncomingDamage(incomingDmg);
        }

        int damageDealt = Mathf.CeilToInt(incomingDmg);
        currentHealth -= damageDealt;
        healthText.text = "Health: " + currentHealth;
        Debug.Log($"{entityName} takes {incomingDmg} damage");

        // Show damage popup via SpawnsDamagePopups singleton
        if (SpawnsDamagePopups.Instance != null)
        {
            SpawnsDamagePopups.Instance.DamageDone(damageDealt, transform.position, false);
        }

        // Animate health bar
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }
        healthBarCoroutine = StartCoroutine(AnimateHealthBar());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log($"{entityName} is dead");
            EntityDead();
        }

        StartCoroutine(turnRed()); 
    }


    IEnumerator turnRed()
    {
        sprite.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(_timing);
        sprite.color = Color.white; 
    }

    IEnumerator AnimateHealthBar()
    {
        float targetScale = (float)currentHealth / maxHealth * healthBarMaxWidth;
        Vector3 startScale = healthBar.GetComponent<Transform>().localScale;
        Vector3 targetScaleVector = new Vector3(targetScale, startScale.y, startScale.z);
        
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Lerp the scale
            healthBar.GetComponent<Transform>().localScale = Vector3.Lerp(startScale, targetScaleVector, t);

            // Update position to keep left-aligned
            float currentScaleX = healthBar.GetComponent<Transform>().localScale.x;
            healthBar.GetComponent<Transform>().localPosition = new Vector3(
                healthBarStartPos.x - (healthBarMaxWidth - currentScaleX) / 2 * healthBar.GetComponent<RectTransform>().rect.width,
                healthBarStartPos.y,
                healthBarStartPos.z
            );

            yield return null;
        }

        // Ensure final values are exact
        healthBar.GetComponent<Transform>().localScale = targetScaleVector;
        healthBar.GetComponent<Transform>().localPosition = new Vector3(
            healthBarStartPos.x - (healthBarMaxWidth - targetScale) / 2 * healthBar.GetComponent<RectTransform>().rect.width,
            healthBarStartPos.y,
            healthBarStartPos.z
        );
    }

}
