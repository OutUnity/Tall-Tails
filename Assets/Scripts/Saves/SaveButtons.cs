using UnityEngine;

public class SaveButtons : MonoBehaviour
{
    [SerializeField] private int slotIndex;

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(slotIndex);
    }

    public void ContinueGame()
    {
        SaveSystem.LoadLatestSave();
    }
}