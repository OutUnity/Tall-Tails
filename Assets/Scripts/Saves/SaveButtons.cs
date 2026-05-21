using UnityEngine;

public class SaveButtons : MonoBehaviour
{
    [Header("Save")]
    [SerializeField] private int slotIndex;

    // =====================================================
    // SET SLOT
    // =====================================================

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    // =====================================================
    // CONTINUE
    // =====================================================

    public void ContinueGame()
    {
        if (GameLoader.Instance != null)
        {
            GameLoader.Instance.ContinueGame();
        }
        else
        {
            Debug.LogError("GameLoader Instance missing.");
        }
    }

    // =====================================================
    // LOAD SLOT
    // =====================================================

    public void LoadGame()
    {
        if (GameLoader.Instance != null)
        {
            GameLoader.Instance.LoadSave(slotIndex);
        }
        else
        {
            Debug.LogError("GameLoader Instance missing.");
        }
    }

    // =====================================================
    // SAVE SLOT
    // =====================================================

    public void SaveGame()
    {
        SaveSystem.SaveGame(slotIndex);
    }
}