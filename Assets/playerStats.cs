using UnityEngine;

public class playerStats : MonoBehaviour
{
    [SerializeField] int _playerHealth = 11;
    [SerializeField] int _maxPlayerHealth = 11;

    [SerializeField] int _playerAttack = 2;
    [SerializeField] int _playerBlock = 5;

    [SerializeField] int _currentBlock = 0; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getHealth()
    {
        return _playerHealth;
    }

    public void setHealth(int newHealth)
    {
        _playerHealth = newHealth;
    }

    public void damageHealth(int damage)
    {
        if (_currentBlock > 0)
        {
            int fullDamage = damage; 
            damage -= _currentBlock;

            if(damage < 0)
            {
                damage = 0;
            }

            _currentBlock -= fullDamage;

            if (_currentBlock < 0)
            {
                _currentBlock = 0;
            }

        }
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

    public void addBlock()
    {
        _currentBlock += _playerBlock; 
    }

    public int getBlock()
    {
        if(_currentBlock == 0)
        {
            return 0;
        } 
        else
        {
            return _currentBlock;
        }
    }
}
