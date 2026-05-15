using UnityEngine;

public class SaveResetOnFirstRun : MonoBehaviour
{
    [Header("Development")]
    [SerializeField] private bool clearSavesOnPlay = true;

    private const int MAX_SLOTS = 10;

    void Awake()
    {
        if (clearSavesOnPlay)
        {
            ClearAllSaves();

            Debug.Log("Development reset completed.");
        }
    }

    private void ClearAllSaves()
    {
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            PlayerPrefs.DeleteKey(
                "SaveSlot_" + i
            );
        }

        PlayerPrefs.DeleteKey("HasSave");

        PlayerPrefs.Save();
    }
}