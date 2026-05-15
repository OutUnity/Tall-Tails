using UnityEngine;
using TMPro;

public class LoadMenuUI : MonoBehaviour
{
    public enum SaveMenuMode
    {
        Load,
        Save
    }

    [Header("Mode")]
    [SerializeField] private SaveMenuMode currentMode;

    [Header("References")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private TMP_Text titleText;

    private void OnEnable()
    {
        Refresh();
    }

    public void OpenLoadMenu()
    {
        currentMode = SaveMenuMode.Load;

        if (titleText != null)
        {
            titleText.text = "LOAD GAME";
        }

        gameObject.SetActive(true);
        Refresh();
    }

    public void OpenSaveMenu()
    {
        currentMode = SaveMenuMode.Save;

        if (titleText != null)
        {
            titleText.text = "SAVE GAME";
        }

        gameObject.SetActive(true);
        Refresh();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        int slotCount = SaveSystem.GetSlotCount();

        for (int i = 0; i < slotCount; i++)
        {
            int index = i;

            GameObject obj = Instantiate(slotPrefab, slotContainer);

            SaveSlotUI ui = obj.GetComponent<SaveSlotUI>();

            SaveSlot slot = SaveSystem.LoadSlot(index);

            // =====================================================
            // LOAD MODE
            // =====================================================

            if (currentMode == SaveMenuMode.Load)
            {
                if (slot == null)
                {
                    ui.SetupEmpty(index, OnSaveClicked);
                }
                else
                {
                    ui.Setup(index, slot);
                }
            }

            // =====================================================
            // SAVE MODE
            // =====================================================

            if (currentMode == SaveMenuMode.Save)
            {
                if (slot == null)
                {
                    ui.SetupEmpty(index, OnSaveClicked);
                }
                else
                {
                    ui.Setup(index, slot);

                    // overwrite behavior (no new function names added)
                    ui.GetComponent<UnityEngine.UI.Button>()
                        .onClick.RemoveAllListeners();

                    ui.GetComponent<UnityEngine.UI.Button>()
                        .onClick.AddListener(() =>
                        {
                            SaveSystem.SaveGame(index);
                            Refresh();
                        });
                }
            }
        }
    }

    private void OnSaveClicked(int index)
    {
        SaveSystem.SaveGame(index);
        Refresh();
    }
}