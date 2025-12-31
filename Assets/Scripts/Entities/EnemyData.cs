using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite art;
    public int maxHealth;
    public int attack;
    public int defense;
    public int force;
    public int shield;
    public int accuracy;
    public int evasion;
    public bool isBoss;
    public CardData[] cards;

    [Header("Battle Script")]
    [Tooltip("Optional battle script for advanced enemy AI")]
    public BattleScript battleScript;

    [Header("Damage Resistances")]
    public DamageResistances damageResistances = new DamageResistances();
}
