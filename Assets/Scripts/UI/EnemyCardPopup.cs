using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class EnemyCardPopup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private RectTransform popupRect;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image backgroundImage;

    [Header("Layout Settings")]
    [Tooltip("Padding on each side of the text")]
    [SerializeField] private float horizontalPadding = 20f;
    [Tooltip("Fixed height of the popup")]
    [SerializeField] private float popupHeight = 40f;
    [Tooltip("Minimum width of the popup")]
    [SerializeField] private float minWidth = 100f;

    [Header("Animation Settings")]
    [Tooltip("Duration of fade-in animation")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [Tooltip("Duration of display time")]
    [SerializeField] private float displayDuration = 1.5f;
    [Tooltip("Duration of fade-out animation")]
    [SerializeField] private float fadeOutDuration = 0.3f;
    [Tooltip("Vertical distance to travel during fade-in (negative = from below)")]
    [SerializeField] private float fadeInDistance = -30f;
    [Tooltip("Vertical distance to travel during fade-out (positive = upward)")]
    [SerializeField] private float fadeOutDistance = 30f;

    [Header("Customization")]
    [SerializeField] private Color backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.9f);
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private TMP_FontAsset textFont;
    [SerializeField] private int fontSize = 18;

    private Vector3 targetPosition;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        // Get or create required components
        if (popupRect == null)
            popupRect = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Apply customization
        ApplyCustomization();

        // Start invisible
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void ApplyCustomization()
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = backgroundColor;
        }

        if (cardNameText != null)
        {
            cardNameText.color = textColor;
            cardNameText.fontSize = fontSize;

            if (textFont != null)
            {
                cardNameText.font = textFont;
            }
        }
    }

    /// <summary>
    /// Display the card name popup with animation
    /// </summary>
    public void ShowCardName(string cardName, Vector3 worldPosition)
    {
        // Stop any existing animation
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        // Set card name
        if (cardNameText != null)
        {
            cardNameText.text = cardName;
        }

        // Calculate size based on text
        AdjustPopupSize();

        // Position the popup
        targetPosition = worldPosition;

        // Start animation
        gameObject.SetActive(true);
        animationCoroutine = StartCoroutine(AnimatePopup());
    }

    private void AdjustPopupSize()
    {
        if (cardNameText == null || popupRect == null)
            return;

        // Force text to update
        cardNameText.ForceMeshUpdate();

        // Get preferred width of text
        float textWidth = cardNameText.preferredWidth;

        // Calculate popup width with padding
        float popupWidth = Mathf.Max(minWidth, textWidth + (horizontalPadding * 2));

        // Set size
        popupRect.sizeDelta = new Vector2(popupWidth, popupHeight);
    }

    private IEnumerator AnimatePopup()
    {
        // === FADE IN ===
        float elapsed = 0f;
        Vector3 startPos = targetPosition + new Vector3(0, fadeInDistance, 0);
        canvasGroup.alpha = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;

            // Smooth fade in
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            // Move from below to target position
            transform.position = Vector3.Lerp(startPos, targetPosition, t);

            yield return null;
        }

        // Ensure final values
        canvasGroup.alpha = 1f;
        transform.position = targetPosition;

        // === DISPLAY ===
        yield return new WaitForSeconds(displayDuration);

        // === FADE OUT ===
        elapsed = 0f;
        Vector3 endPos = targetPosition + new Vector3(0, fadeOutDistance, 0);
        startPos = targetPosition;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;

            // Smooth fade out
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            // Move upward
            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        // Ensure final values
        canvasGroup.alpha = 0f;
        transform.position = endPos;

        // Deactivate
        gameObject.SetActive(false);
        animationCoroutine = null;
    }

    /// <summary>
    /// Immediately hide the popup
    /// </summary>
    public void Hide()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    // Public setters for runtime customization
    public void SetBackgroundColor(Color color)
    {
        backgroundColor = color;
        if (backgroundImage != null)
            backgroundImage.color = color;
    }

    public void SetTextColor(Color color)
    {
        textColor = color;
        if (cardNameText != null)
            cardNameText.color = color;
    }

    public void SetFontSize(int size)
    {
        fontSize = size;
        if (cardNameText != null)
            cardNameText.fontSize = size;
    }

    public void SetAnimationDurations(float fadeIn, float display, float fadeOut)
    {
        fadeInDuration = fadeIn;
        displayDuration = display;
        fadeOutDuration = fadeOut;
    }

    public void SetAnimationDistances(float fadeInDist, float fadeOutDist)
    {
        fadeInDistance = fadeInDist;
        fadeOutDistance = fadeOutDist;
    }
}
