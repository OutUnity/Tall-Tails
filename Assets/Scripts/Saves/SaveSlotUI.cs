using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text regionText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text playtimeText;

    [SerializeField] private Button button;

    // =====================================================
    // SETUP
    // =====================================================
    public void Setup(int index, SaveSlot slot)
    {
        // REGION
        if (regionText != null)
        {
            regionText.text =
                RegionUI.Instance.GetRegionName(slot.regionID);
        }

        // DATE
        if (dateText != null)
        {
            dateText.text = slot.saveDate;
        }

        // PLAYTIME
        if (playtimeText != null)
        {
            playtimeText.text =
                FormatPlaytime(slot.playTime);
        }

        // BUTTON
        if (button != null)
        {
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() =>
            {
                SaveSystem.LoadGameFromSlot(index);
            });
        }
    }

    // =====================================================
    // FORMAT PLAYTIME
    // =====================================================
    string FormatPlaytime(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);
        int minutes = Mathf.FloorToInt((seconds % 3600) / 60);

        return $"{hours}h {minutes}m";
    }
}