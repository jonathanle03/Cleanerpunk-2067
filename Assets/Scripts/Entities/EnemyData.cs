using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite art;
    public int maxHealth;
    public int attack;
    public int defense;
    public bool isBoss;
    public CardData[] cards;
}
