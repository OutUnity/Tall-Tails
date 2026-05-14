using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadMenuUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject loadPanel;

    void OnEnable()
    {
        Refresh();
    }

    public void Open()
    {
        loadPanel.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        loadPanel.SetActive(false);
    }

    public void Refresh()
    {
        // Clear old UI
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        // ---------------------------
        // NEW SAVE BUTTON (TOP)
        // ---------------------------
        GameObject newSave = Instantiate(slotPrefab, slotContainer);

        TMP_Text[] newSaveTexts = newSave.GetComponentsInChildren<TMP_Text>();
        newSaveTexts[0].text = "NEW SAVE";
        newSaveTexts[1].text = "";

        newSave.GetComponent<Button>().onClick.AddListener(() =>
        {
            SaveSystem.SaveGame();
            Refresh();
        });

        // ---------------------------
        // LOAD EXISTING SLOTS
        // ---------------------------
        List<(int index, SaveSlot slot)> slots = SaveSystem.GetAllSlots();

        foreach (var data in slots)
        {
            GameObject obj = Instantiate(slotPrefab, slotContainer);

            SaveSlot slot = data.slot;
            int index = data.index;

            TMP_Text[] texts = obj.GetComponentsInChildren<TMP_Text>();

            texts[0].text =
                "Region: " + RegionUI.Instance.GetRegionName(slot.regionID);

            texts[1].text = slot.saveDate;

            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                SaveSystem.LoadGameFromSlot(index);
            });
        }
    }
}