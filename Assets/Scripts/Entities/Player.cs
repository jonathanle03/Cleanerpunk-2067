using UnityEngine;

public class Player : MonoBehaviour
{
    public const int maxMana = 9;
    public int currentMana;
    
    void Start()
    {
        currentMana = maxMana;
    }
}
