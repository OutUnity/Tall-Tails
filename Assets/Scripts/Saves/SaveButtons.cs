using UnityEngine;

public class SaveButtons : MonoBehaviour
{
    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void ContinueGame()
    {
        SaveSystem.LoadLatestSave();
    }
}