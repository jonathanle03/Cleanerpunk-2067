using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[5, 5];
    public float coins;
    public TMP_Text CoinsTXT;

   void Start()
    {
        CoinsTXT.text = "Coins:" + coins.ToString();

        //ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //ID's
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
    }

    public void Buy()
    {
        Debug.Log("I ran.");
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItems[2, ButtonRef.GetComponent<ButtonInput>().ItemID])
        {
            coins -= shopItems[2, ButtonRef.GetComponent<ButtonInput>().ItemID];
            shopItems[3, ButtonRef.GetComponent<ButtonInput>().ItemID]++;
            CoinsTXT.text = "Coins:" + coins.ToString();
            ButtonRef.GetComponent<ButtonInput>().QuantityTxt.text = shopItems[3, ButtonRef.GetComponent<ButtonInput>().ItemID].ToString();

        }

    }
}
