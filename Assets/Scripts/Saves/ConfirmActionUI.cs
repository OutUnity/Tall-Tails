using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmActionUI : MonoBehaviour
{
    public enum ConfirmMode
    {
        SaveOverwrite,
        LoadGame
    }

    [Header("Root")]
    [SerializeField] private GameObject root;

    [Header("Buttons")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("UI")]
    [SerializeField] private TMP_Text messageText;

    private int pendingSlotIndex;

    private System.Action<int> confirmCallback;

    private ConfirmMode currentMode;

    // =====================================================
    // OPEN
    // =====================================================

    public void Open(
        int slotIndex,
        ConfirmMode mode,
        System.Action<int> onConfirm
    )
    {
        pendingSlotIndex = slotIndex;

        currentMode = mode;

        confirmCallback = onConfirm;

        UpdateMessage();

        if (root != null)
        {
            root.SetActive(true);
        }

        if (yesButton != null)
        {
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(OnConfirm);
        }

        if (noButton != null)
        {
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(Close);
        }
    }

    // =====================================================
    // UPDATE MESSAGE
    // =====================================================

    private void UpdateMessage()
    {
        if (messageText == null)
        {
            return;
        }

        switch (currentMode)
        {
            case ConfirmMode.SaveOverwrite:

                messageText.text = "Overwrite this save?";

                break;

            case ConfirmMode.LoadGame:

                messageText.text = "Load this save?";

                break;
        }
    }

    // =====================================================
    // CONFIRM
    // =====================================================

    private void OnConfirm()
    {
        if (confirmCallback != null)
        {
            confirmCallback(pendingSlotIndex);
        }

        Close();
    }

    // =====================================================
    // CLOSE
    // =====================================================

    public void Close()
    {
        if (root != null)
        {
            root.SetActive(false);
        }
    }
}