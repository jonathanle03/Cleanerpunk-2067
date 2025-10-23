using UnityEngine;

public class playerStats : MonoBehaviour
{
    // We can adjust max health from the inspector. On Start(), the class itself initializes the current health. 
    private int _playerHealth;
    
    // Stats. 
    [SerializeField] int _maxPlayerHealth = 11;
    [SerializeField] int _playerAttack = 2;
    [SerializeField] int _playerBlock = 5;

    // This is to track how much block is on the player currently. 
    [SerializeField] int _currentBlock = 0;


    private void Start()
    {
        // We apply the max health to the current health. 
        _playerHealth = _maxPlayerHealth;
    }

    // health getter, hopefully these get/sets are pretty self explanatory. 
    public int getHealth()
    {
        return _playerHealth;
    }

    public void setHealth(int newHealth)
    {
        _playerHealth = newHealth;
    }

    // This is called when enemy does damage. Does some calculations (in comparison to setHealth, which is currently only used to reset to 0) 
    public void damageHealth(int damage)
    {
        // Checking if there's any block on player
        if (_currentBlock > 0)
        {
            // Store the full damage number, then subtract block from damage. 
            int fullDamage = damage; 
            damage -= _currentBlock;

            // Check if damage was completely nullified- if it's a negative, we want to reset to 0 so the player doesn't heal. 
            if(damage < 0)
            {
                damage = 0;
            }

            // Take the damage stat out of block
            _currentBlock -= fullDamage;

            // Similar to damage, we don't want block to be negative if it was fully taken out. Reset it to 0. 
            if (_currentBlock < 0)
            {
                _currentBlock = 0;
            }

        }

        // Once the block calculations are done, subtract the final damage from player health. 
        _playerHealth -= damage;
    }

    public int getMaxHealth()
    {
        return _maxPlayerHealth;
    }

    public int getAttack()
    {
        return _playerAttack;
    }

    // Called when the player hits the block button, 
    public void addBlock()
    {
        _currentBlock += _playerBlock; 
    }

    // This is for the UpdateUI script. 
    public int getBlock()
    {
        return _currentBlock; 
    }
}
