using UnityEngine;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    // These fields are set by the ShopManager script after instantiation.
    [HideInInspector] public ItemData itemData;
    [HideInInspector] public ShopManager shopManager;

    // UI References (Set by ShopManager)
    // NOTE: Ensure your prefab has children named "NameText", "PriceText", and "QuantityText"
    [HideInInspector] public TextMeshProUGUI nameText;
    [HideInInspector] public TextMeshProUGUI priceText;
    [HideInInspector] public TextMeshProUGUI quantityText;

    // Called by the ShopManager to refresh the button's visual state
    public void UpdateDisplay()
    {
        // Static Data
        nameText.text = itemData.itemName;
        priceText.text = itemData.basePrice.ToString();

        // Dynamic Data (Get from Inventory Manager)
        int ownedQuantity = shopManager.GetInventoryManager().GetQuantity(itemData);
        quantityText.text = $"Owned: {ownedQuantity}";
    }

    // This method is called via the ShopManager's script-based AddListener
    public void OnBuyClicked()
    {
        if (shopManager.CanAfford(itemData.basePrice))
        {
            // 1. Process purchase (deduct balance)
            shopManager.DeductBalance(itemData.basePrice);

            // 2. Update player inventory
            shopManager.GetInventoryManager().AddItem(itemData);

            // 3. Update the display immediately
            UpdateDisplay();

            Debug.Log($"Purchased {itemData.itemName} for {itemData.basePrice}.");
        }
        else
        {
            Debug.Log($"Not enough money to buy {itemData.itemName}!");
        }
    }
}