using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Dictionary to store ItemData (the key) and the quantity owned (the value)
    private Dictionary<ItemData, int> _itemQuantities = new Dictionary<ItemData, int>();

    public void AddItem(ItemData item, int count = 1)
    {
        if (item == null) return;

        if (_itemQuantities.ContainsKey(item))
        {
            _itemQuantities[item] += count;
        }
        else
        {
            _itemQuantities.Add(item, count);
        }

        // **IMPORTANT:** In a real game, you'd trigger a UI refresh event here!
    }

    public int GetQuantity(ItemData item)
    {
        if (item == null) return 0;

        if (_itemQuantities.ContainsKey(item))
        {
            return _itemQuantities[item];
        }
        return 0;
    }
}