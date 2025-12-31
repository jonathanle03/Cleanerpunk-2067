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

    // Card interactivity state
    private CanvasGroup canvasGroup;
    private bool isInteractable = true; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get or add CanvasGroup component for alpha control
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
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
        image.texture = card.art;

        switch (card.type)
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

    }

    /// <summary>
    /// Disable card interaction and grey it out visually
    /// </summary>
    public void DisableCard()
    {
        isInteractable = false;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.5f; // Grey out to 50% opacity
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Enable card interaction and restore normal appearance
    /// </summary>
    public void EnableCard()
    {
        isInteractable = true;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; // Full opacity
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// Check if the card is currently interactable
    /// </summary>
    public bool IsInteractable()
    {
        return isInteractable;
    }
}
