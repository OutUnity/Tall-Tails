using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StyledToggleText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private TextMeshProUGUI label;
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
        if (toggle == null || label == null || style == null)
            return;

        // Store original scale
        originalScale = transform.localScale;

        // Create unique material instance
        materialInstance = new Material(label.fontMaterial);
        label.fontMaterial = materialInstance;
        
        UpdateVisual();
    }

    void OnEnable()
    {

        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        ResetVisuals();
    }

    void OnDisable()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }

    // --------------------------------------------------
    // TOGGLE CHANGED
    // --------------------------------------------------
    void OnToggleChanged(bool value)
    {
        UpdateVisual();
    }

    // --------------------------------------------------
    // UPDATE VISUAL STATE
    // --------------------------------------------------
    void UpdateVisual()
    {
        if (toggle == null || label == null || style == null)
            return;

        // Active / inactive colors
        if (toggle.isOn)
        {
            label.color = style.activeColor;

            if (useGlow)
                ApplyGlow();
        }
        else
        {
            label.color = style.defaultColor;

            RemoveGlow();
        }
        if(useBold || useUnderline)
        {
            ApplyTextStyle();
        }
    }

    // --------------------------------------------------
    // HOVER ENTER
    // --------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggle == null || label == null || style == null)
            return;

        // Hover color
        label.color = style.hoverColor;

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

        // Hover text style
        ApplyTextStyle();
    }

    // --------------------------------------------------
    // HOVER EXIT
    // --------------------------------------------------
    public void OnPointerExit(PointerEventData eventData)
    {
        if (toggle == null || label == null || style == null)
            return;

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
    void ResetVisuals()
    {
        if (toggle == null || label == null || style == null)
            return;

        // Reset scale
        if (useScale)
        {
            transform.localScale = originalScale;
        }

        // Remove temporary hover text styling
        label.fontStyle = FontStyles.Normal;

        // Restore proper toggle visuals
        UpdateVisual();
    }
    void ApplyTextStyle()
    {
        if (label == null)
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

        label.fontStyle = styles;
    }

}