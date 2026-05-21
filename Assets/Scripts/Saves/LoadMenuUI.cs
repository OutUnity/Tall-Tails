using UnityEngine;
using UnityEngine.UI;
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

    [Header("Systems")]
    [SerializeField] private ConfirmActionUI confirmActionUI;

    private void OnEnable()
    {
        SaveSystem.OnBeforeScreenshot += HideUIForCapture;
        Refresh();
    }

    // =====================================================
    // OPEN LOAD MENU
    // =====================================================
    private void OnDisable()
    {
        SaveSystem.OnBeforeScreenshot -= HideUIForCapture;
    }

    private void HideUIForCapture()
    {
        gameObject.SetActive(false); // or just canvasGroup.alpha = 0
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

    // =====================================================
    // OPEN SAVE MENU
    // =====================================================

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

    // =====================================================
    // CLOSE
    // =====================================================

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    // =====================================================
    // REFRESH
    // =====================================================

    public void Refresh()
    {
        if (slotContainer == null)
        {
            Debug.LogError("Slot Container missing.");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("Slot Prefab missing.");
            return;
        }

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

            if (ui == null)
            {
                Debug.LogError("SaveSlotUI missing on SlotPrefab.");
                continue;
            }

            SaveSlot slot = SaveSystem.LoadSlot(index);

            // =====================================================
            // LOAD MODE
            // =====================================================

            if (currentMode == SaveMenuMode.Load)
            {
                SetupLoadMode(ui, slot, index);
            }

            // =====================================================
            // SAVE MODE
            // =====================================================

            if (currentMode == SaveMenuMode.Save)
            {
                SetupSaveMode(ui, slot, index);
            }
        }
    }

    // =====================================================
    // LOAD MODE SETUP
    // =====================================================

    private void SetupLoadMode(SaveSlotUI ui, SaveSlot slot, int index)
    {
        // EMPTY SLOT
        if (slot == null)
        {
            ui.SetupEmpty(index, null);

            Button emptyButton = ui.GetComponent<Button>();

            if (emptyButton != null)
            {
                emptyButton.interactable = false;
            }

            return;
        }

        // SAVED SLOT
        ui.Setup(index, slot);

        Button slotButton = ui.GetComponent<Button>();

        if (slotButton != null)
        {
            slotButton.onClick.RemoveAllListeners();

            slotButton.onClick.AddListener(() =>
            {
                if (confirmActionUI != null)
                {
                    confirmActionUI.Open(index, ConfirmActionUI.ConfirmMode.LoadGame, OnLoadConfirmed);
                }
            });
        }
    }

    // =====================================================
    // SAVE MODE SETUP
    // =====================================================

    private void SetupSaveMode(SaveSlotUI ui, SaveSlot slot, int index)
    {
        // EMPTY SLOT
        if (slot == null)
        {
            ui.SetupEmpty(index, OnSaveClicked);

            return;
        }

        // SAVED SLOT
        ui.Setup(index, slot);

        Button slotButton = ui.GetComponent<Button>();

        if (slotButton != null)
        {
            slotButton.onClick.RemoveAllListeners();

            slotButton.onClick.AddListener(() =>
            {
                if (confirmActionUI != null)
                {
                    confirmActionUI.Open(index, ConfirmActionUI.ConfirmMode.SaveOverwrite, OnOverwriteConfirmed);
                }
            });
        }
    }

    // =====================================================
    // SAVE NEW
    // =====================================================

    private void OnSaveClicked(int index)
    {
        SaveSystem.SaveGame(index, Refresh);
    }

    // =====================================================
    // OVERWRITE
    // =====================================================

    private void OnOverwriteConfirmed(int index)
    {
        SaveSystem.SaveGame(index, Refresh);
    }

    // =====================================================
    // LOAD
    // =====================================================

    private void OnLoadConfirmed(int index)
    {
        if (GameLoader.Instance != null)
        {
            GameLoader.Instance.LoadSave(index);
        }
        else
        {
            Debug.LogError("GameLoader Instance missing.");
        }
    }
}