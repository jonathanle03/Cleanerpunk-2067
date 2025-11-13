using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class cardReader : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] RawImage backing;
    [SerializeField] RawImage image; 
    

    public CardData card; 





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setCard()
    {
        cardName.text = card.cardName;
        description.text = card.description;
        cost.text = card.cost.ToString();
        //image.texture = card.art;
    }
}
