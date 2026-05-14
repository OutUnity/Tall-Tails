using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverTextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private UITextStyle style;

    [Header("Effects (Per Element Control)")]
    [SerializeField] private bool useGlow = false;
    [SerializeField] private bool useScale = false;
    [SerializeField] private bool useBold = false;
    [SerializeField] private bool useUnderline = false;

    private Vector3 originalScale;
    private Material materialInstance;

    void Awake()
    {
        if (text == null || style == null)
            return;

        // Store original scale
        originalScale = transform.localScale;

        // Create unique material instance
        materialInstance = new Material(text.fontMaterial);
        text.fontMaterial = materialInstance;

        // Set starting visuals
        text.color = style.defaultColor;

        RemoveGlow();

        // Ensure no hover styling exists at start
        text.fontStyle = FontStyles.Normal;
    }

    // --------------------------------------------------
    // HOVER ENTER
    // --------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (text == null || style == null)
            return;

        // Hover color
        text.color = style.hoverColor;

        // Hover scale
        if (useScale)
        {
            transform.localScale =
                originalScale * style.hoverScale;
        }

        // Hover glow
        if (useGlow)
        {
            ApplyGlow();
        }

        // Hover text styling
        ApplyTextStyle();
    }

    // --------------------------------------------------
    // HOVER EXIT
    // --------------------------------------------------
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetVisuals();
    }

    // --------------------------------------------------
    // GLOW
    // --------------------------------------------------
    void ApplyGlow()
    {
        if (materialInstance == null)
            return;

        materialInstance.SetColor(
            "_GlowColor",
            style.glowColor
        );

        materialInstance.SetFloat(
            "_GlowPower",
            style.glowIntensity
        );
    }

    void RemoveGlow()
    {
        if (materialInstance == null)
            return;

        materialInstance.SetFloat(
            "_GlowPower",
            0f
        );
    }

    // --------------------------------------------------
    // DISABLE
    // --------------------------------------------------
    void OnDisable()
    {
        ResetVisuals();
    }

    // --------------------------------------------------
    // RESET VISUALS
    // --------------------------------------------------
    void ResetVisuals()
    {
        if (text == null || style == null)
            return;

        // Reset color
        text.color = style.defaultColor;

        // Reset scale
        if (useScale)
        {
            transform.localScale = originalScale;
        }

        // Remove glow
        RemoveGlow();

        // Remove temporary hover styling
        text.fontStyle = FontStyles.Normal;
    }

    // --------------------------------------------------
    // TEXT STYLE
    // --------------------------------------------------
    void ApplyTextStyle()
    {
        if (text == null)
            return;

        FontStyles styles = FontStyles.Normal;

        if (useBold)
        {
            styles |= FontStyles.Bold;
        }

        if (useUnderline)
        {
            styles |= FontStyles.Underline;
        }

        text.fontStyle = styles;
    }
}