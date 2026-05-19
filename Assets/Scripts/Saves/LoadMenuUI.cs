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

    [Header("Confirmation UI")]
    [SerializeField] private ConfirmActionUI confirmActionUI;

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

            GameObject obj =
                Instantiate(slotPrefab, slotContainer);

            SaveSlotUI ui =
                obj.GetComponent<SaveSlotUI>();

            SaveSlot slot =
                SaveSystem.LoadSlot(index);

            // =====================================================
            // LOAD MODE
            // =====================================================

            if (currentMode == SaveMenuMode.Load)
            {
                // EMPTY SLOT
                if (slot == null)
                {
                    ui.SetupEmpty(index, OnSaveClicked);

                    Button emptyButton =
                        ui.GetComponent<Button>();

                    if (emptyButton != null)
                    {
                        emptyButton.interactable = false;
                    }
                }

                // SAVED SLOT
                else
                {
                    ui.Setup(index, slot);

                    Button slotButton =
                        ui.GetComponent<Button>();

                    if (slotButton != null)
                    {
                        slotButton.onClick.RemoveAllListeners();

                        slotButton.onClick.AddListener(() =>
                        {
                            confirmActionUI.Open(
                                index,
                                ConfirmActionUI.ConfirmMode.LoadGame,
                                OnLoadConfirmed
                            );
                        });
                    }
                }
            }

            // =====================================================
            // SAVE MODE
            // =====================================================

            if (currentMode == SaveMenuMode.Save)
            {
                // EMPTY SLOT
                if (slot == null)
                {
                    ui.SetupEmpty(index, OnSaveClicked);
                }

                // SAVED SLOT
                else
                {
                    ui.Setup(index, slot);

                    Button slotButton =
                        ui.GetComponent<Button>();

                    if (slotButton != null)
                    {
                        slotButton.onClick.RemoveAllListeners();

                        slotButton.onClick.AddListener(() =>
                        {
                            confirmActionUI.Open(
                                index,
                                ConfirmActionUI.ConfirmMode.SaveOverwrite,
                                OnOverwriteConfirmed
                            );
                        });
                    }
                }
            }
        }
    }

    private void OnSaveClicked(int index)
    {
        SaveSystem.SaveGame(index);

        Refresh();
    }

    private void OnOverwriteConfirmed(int index)
    {
        SaveSystem.SaveGame(index);

        Refresh();
    }

    private void OnLoadConfirmed(int index)
    {
        SaveSystem.LoadGameFromSlot(index);
    }
}