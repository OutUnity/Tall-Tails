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
    [SerializeField] private bool useScale = true;

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

        // Glow
        if (useGlow)
        {
            ApplyGlow();
        }
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
    void OnDisable()
    {
        ResetVisuals();
    }
    void ResetVisuals()
    {
        if (text == null || style == null)
            return;

        text.color = style.defaultColor;

        if (useScale)
        {
            transform.localScale = originalScale;
        }

        RemoveGlow();
    }
}