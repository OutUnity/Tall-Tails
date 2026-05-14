using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverPanelHighlight : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Image panelImage;
    [SerializeField] private UITextStyle style;

    [Header("Effects")]
    [SerializeField] private bool useScale = false;
    [SerializeField] private bool useGlow = false;

    [Header("Panel Colors")]
    [SerializeField]
    private Color defaultPanelColor =
        new Color(1f, 1f, 1f, 0.08f);

    [SerializeField]
    private Color hoverPanelColor =
        new Color(1f, 1f, 1f, 0.18f);

    [Header("Outline")]
    [SerializeField] private bool useOutline = false;
    [SerializeField] private Outline outline;

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;

        ResetVisuals();
    }

    // --------------------------------------------------
    // HOVER ENTER
    // --------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panelImage != null)
        {
            panelImage.color = hoverPanelColor;
        }

        if (useScale && style != null)
        {
            transform.localScale =
                originalScale * style.hoverScale;
        }

        if (useOutline && outline != null)
        {
            outline.enabled = true;

            if (style != null)
            {
                outline.effectColor = style.glowColor;
            }
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
    // RESET
    // --------------------------------------------------
    void ResetVisuals()
    {
        if (panelImage != null)
        {
            panelImage.color = defaultPanelColor;
        }

        transform.localScale = originalScale;

        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    // --------------------------------------------------
    // DISABLE SAFETY
    // --------------------------------------------------
    void OnDisable()
    {
        ResetVisuals();
    }
}