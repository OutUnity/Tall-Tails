using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text regionText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text playtimeText;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject infoContainer;
    [SerializeField] private Image screenshotImage;

    [Header("Layout")]
    [SerializeField] private HorizontalLayoutGroup infoLayoutGroup;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

    [Header("Font Sizes")]
    [SerializeField] private int emptySlotFontSize = 50;
    [SerializeField] private int savedSlotFontSize = 24;

    [Header("Padding")]
    [SerializeField] private int savedSlotLeftPadding = 30;
    [SerializeField] private int emptySlotLeftPadding = 0;

    [SerializeField] private int savedSlotTopPadding = 0;
    [SerializeField] private int emptySlotTopPadding = 30;

    [Header("Colors")]
    [SerializeField]
    private Color savedTextColor = Color.white;

    [SerializeField]
    private Color emptyTextColor = new Color(1f, 1f, 1f, 0.6f);

    private int slotIndex;

    // =====================================================
    // SAVED SLOT
    // =====================================================
    public void Setup(int index, SaveSlot slot)
    {
        ResetUI();

        slotIndex = index;

        // REGION
        regionText.text = RegionDatabase.GetName(slot.regionID);

        regionText.alignment = TextAlignmentOptions.Left;

        regionText.fontSize = savedSlotFontSize;

        regionText.color = savedTextColor;

        // DATE
        dateText.text = slot.saveDate;

        dateText.color = savedTextColor;

        // PLAYTIME
        playtimeText.text = FormatPlaytime(slot.playTime);

        playtimeText.color = savedTextColor;

        // SCREENSHOT
        if (screenshotImage != null)
        {
            screenshotImage.gameObject.SetActive(true);
        }

        // HORIZONTAL LAYOUT
        if (infoLayoutGroup != null)
        {
            infoLayoutGroup.childAlignment = TextAnchor.MiddleLeft;

            infoLayoutGroup.padding.left = savedSlotLeftPadding;
        }

        // VERTICAL LAYOUT
        if (verticalLayoutGroup != null)
        {
            verticalLayoutGroup.padding.top = savedSlotTopPadding;

            verticalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        }

        // BUTTON
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            SaveSystem.LoadGameFromSlot(slotIndex);
        });
    }

    // =====================================================
    // EMPTY SLOT
    // =====================================================
    public void SetupEmpty(int index, System.Action<int> onSave)
    {
        ResetUI();

        slotIndex = index;

        // REGION
        regionText.text = "NEW SAVE";

        regionText.alignment = TextAlignmentOptions.Center;

        regionText.fontSize = emptySlotFontSize;
        regionText.color = emptyTextColor;

        // DATE
        dateText.text = "";

        dateText.color = emptyTextColor;

        // PLAYTIME
        playtimeText.text = "";

        playtimeText.color = emptyTextColor;

        // HIDE SCREENSHOT
        if (screenshotImage != null)
        {
            screenshotImage.gameObject.SetActive(false);
        }

        // HORIZONTAL LAYOUT
        if (infoLayoutGroup != null)
        {
            infoLayoutGroup.childAlignment = TextAnchor.MiddleCenter;

            infoLayoutGroup.padding.left = emptySlotLeftPadding;
        }

        // VERTICAL LAYOUT
        if (verticalLayoutGroup != null)
        {
            verticalLayoutGroup.padding.top = emptySlotTopPadding;

            verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        }

        // BUTTON
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() =>
        {
            onSave(index);
        });
    }

    // =====================================================
    // RESET UI
    // =====================================================
    public void ResetUI()
    {
        // REGION TEXT
        if (regionText != null)
        {
            regionText.text = "";
            regionText.gameObject.SetActive(true);
        }

        // DATE TEXT
        if (dateText != null)
        {
            dateText.text = "";
            dateText.gameObject.SetActive(true);
        }

        // PLAYTIME TEXT
        if (playtimeText != null)
        {
            playtimeText.text = "";
            playtimeText.gameObject.SetActive(true);
        }

        // SCREENSHOT
        if (screenshotImage != null)
        {
            screenshotImage.sprite = null;
            screenshotImage.gameObject.SetActive(true);
        }

        // INFO CONTAINER
        if (infoContainer != null)
        {
            infoContainer.SetActive(true);
        }
    }

    // =====================================================
    // FORMAT PLAYTIME
    // =====================================================
    private string FormatPlaytime(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);

        int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
        return hours + "h " + minutes + "m";
    }

    // =====================================================
    // REGION DATABASE
    // =====================================================
    public static class RegionDatabase
    {
        public static string GetName(int id)
        {
            switch (id)
            {
                case 0:
                    return "Main Menu";

                case 1:
                    return "Whispering Grove";

                case 2:
                    return "Crystal Falls";

                case 3:
                    return "Sunlit Meadow";

                case 4:
                    return "Azure Ridge";

                case 5:
                    return "Mosswood Hollow";

                case 6:
                    return "Golden Highlands";

                case 7:
                    return "Starlight Basin";

                case 8:
                    return "Credits";

                default:
                    return "Main Menu";
            }
        }
    }
}