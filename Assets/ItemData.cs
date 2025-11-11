using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName = "Default Item";
    public int basePrice = 10;

    /*
    // Additional optional properties
    public Sprite itemIcon;
    [TextArea]
    public string description = "A useful item.";
    */
}