using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [Header("Shop References (Inspector Setup)")]
    public TextMeshProUGUI balanceDisplay;
    public Transform shopContentParent;
    public GameObject shopItemButtonPrefab; // The prefab containing ShopItemButton.cs and Button component

    [Header("Data References")]
    public List<ItemData> availableItems; // List of ScriptableObjects to sell
    public InventoryManager inventoryManager; // Must be linked in the Inspector (or found via code/Singleton)

    private int _currentBalance = 100; // Starting Balance

    private void Start()
    {
        // Basic safety check for the required manager
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not set on ShopManager! Cannot initialize shop.");
            return;
        }

        UpdateBalanceDisplay();
        InstantiateShopButtons();
    }

    private void InstantiateShopButtons()
    {
        foreach (var item in availableItems)
        {
            // 1. Instantiate the Prefab
            GameObject buttonGO = Instantiate(shopItemButtonPrefab, shopContentParent);

            // 2. Locate the necessary components on the new buttonGO
            ShopItemButton shopButtonScript = buttonGO.GetComponent<ShopItemButton>();
            Button buttonComponent = buttonGO.GetComponent<Button>();

            // NOTE: We assume these components are named precisely as below!
            TextMeshProUGUI nameText = buttonGO.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = buttonGO.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI quantityText = buttonGO.transform.Find("QuantityText")?.GetComponent<TextMeshProUGUI>();

            if (shopButtonScript != null && buttonComponent != null && nameText != null)
            {
                // A. Pass the Item Data and Manager Reference
                shopButtonScript.itemData = item;
                shopButtonScript.shopManager = this;

                // B. Set the Text References 
                shopButtonScript.nameText = nameText;
                shopButtonScript.priceText = priceText;
                shopButtonScript.quantityText = quantityText;

                // C. Set Initial Display
                shopButtonScript.UpdateDisplay();

                // D. Script the OnClick Listener!
                // We use a lambda expression () => {} to wrap the call and capture the instance correctly.
                buttonComponent.onClick.AddListener(() => shopButtonScript.OnBuyClicked());
            }
            else
            {
                Debug.LogError($"Prefab setup error for {item.itemName}. Check for missing components or incorrect child names (NameText, PriceText, QuantityText).");
            }
        }
    }

    // --- Public Logic Functions ---
    public bool CanAfford(int price) => _currentBalance >= price;

    public void DeductBalance(int amount)
    {
        _currentBalance -= amount;
        UpdateBalanceDisplay();
    }

    public InventoryManager GetInventoryManager() => inventoryManager;

    public void UpdateBalanceDisplay()
    {
        balanceDisplay.text = $"Balance: ${inventoryManager.GetQuantity(null)}"; // Example: using GetQuantity(null) for dynamic balance.
    }
}