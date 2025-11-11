using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopCards : MonoBehaviour
{
    public TMP_Text moneyText;
    public int moneyLeft;
    public ButtonTest[] cards = new ButtonTest[1];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyText.text = "Balance: " + moneyLeft.ToString();
        for (int i = 0; i < cards.Length; i++)
        {
            System.Random r = new System.Random();
            Card card = gameObject.AddComponent<Card>();
            card.name = "mouse";
            card.cost = r.Next(1, 10);
            ButtonTest b = gameObject.AddComponent<ButtonTest>();
            b.card = card;
            b.setup();
            //ButtonTest.Instantiate(b, new Vector3(i * -100, 0, 0), new Quaternion());
        }
    }

    public void Update()
    {
        moneyText.text = "Balance: " + moneyLeft.ToString();
    }

    // Update is called once per frame
    public void SubtractMoney(int cost)
    {
        if (cost <= moneyLeft)
        {
            moneyLeft -= cost;
        }
        
        
    }
}
