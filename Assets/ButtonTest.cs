using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text cost_display;
    public TMP_Text quantity_display;
    public Card card;
    public ShopCards shop;
    public void Start()
    {
        Debug.Log("Started");
        cost_display.text = "$" + card.cost.ToString();
        quantity_display.text = card.numOwned.ToString();

    }

    public void setup()
    {
        cost_display.text = "$" + card.cost.ToString();
        quantity_display.text = card.numOwned.ToString();
    }

    public void Clicky()
    {
        Debug.Log("Clicked");
        if (card.cost <= shop.moneyLeft)
        {
            card.incrementNumOwned();
            quantity_display.text = card.numOwned.ToString();
            cost_display.text = "$" + card.cost.ToString();
            shop.SubtractMoney(card.cost);
        }
        else
        {
            Debug.Log("Cannot Afford. :(");
        }
    }
}
