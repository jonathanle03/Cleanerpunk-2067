using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class cardReader : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI type;
    [SerializeField] RawImage icon; 
    [SerializeField] RawImage backing;
    [SerializeField] RawImage image;

    [Header("Assets")]
    [SerializeField] Texture[] typeBackings;
    [SerializeField] Texture[] typeIcons;
    

    public CardData card; 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCard()
    {
        cardName.text = card.cardName;
        description.text = card.description;
        cost.text = card.cost.ToString();
        
        switch(card.type)
        {
            case CardType.Offensive:
                backing.texture = typeBackings[0];
                icon.texture = typeIcons[0];
                type.text = "Attack"; 
                break;
            case CardType.Defensive:
                backing.texture = typeBackings[1];
                icon.texture = typeIcons[1];
                type.text = "Defense"; 
                break;
            case CardType.Support:
                backing.texture = typeBackings[2];
                icon.texture = typeIcons[2];
                type.text = "Support"; 
                break;
            default:
                Debug.Log("Something went wrong with type applying.");
                break;
        }
        
        //image.texture = card.art;
    }
}
