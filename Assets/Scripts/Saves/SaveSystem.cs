using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private const int MAX_SLOTS = 10;

    // =========================================================
    // PENDING LOAD (scene-safe transfer)
    // =========================================================
    public static SaveSlot PendingLoad;

    public static bool HasPendingLoad()
    {
        return PendingLoad != null;
    }

    public static SaveSlot ConsumePendingLoad()
    {
        SaveSlot slot = PendingLoad;
        PendingLoad = null;
        return slot;
    }

    // =========================================================
    // SAVE GAME INTO SLOT
    // =========================================================
    public static void SaveGame(int slotIndex)
    {
        SaveSlot slot = BuildSaveSlot(slotIndex);

        slot.screenshotPath = CaptureScreenshot(slotIndex);

        PlayerPrefs.SetString(
            "SaveSlot_" + slotIndex,
            JsonUtility.ToJson(slot)
        );

        PlayerPrefs.Save();
    }

    // =========================================================
    // BUILD SAVE DATA
    // =========================================================
    private static SaveSlot BuildSaveSlot(int slotIndex)
    {
        SaveSlot slot = new SaveSlot();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Vector3 pos = player.transform.position;

            slot.playerX = pos.x;
            slot.playerY = pos.y;
            slot.playerZ = pos.z;

            if (RegionMapManager.Instance != null)
            {
                slot.regionID =
                    RegionMapManager.Instance.GetRegionFromPosition(pos);
            }
        }

        slot.saveDate =
            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (PlayTimeManager.Instance != null)
        {
            slot.playTime =
                PlayTimeManager.Instance.GetPlayTime();
        }

        return slot;
    }

    // =========================================================
    // LOAD SINGLE SLOT DATA
    // =========================================================
    public static SaveSlot LoadSlot(int index)
    {
        string json = PlayerPrefs.GetString("SaveSlot_" + index, "");

        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        SaveSlot slot = JsonUtility.FromJson<SaveSlot>(json);

        if (slot == null)
        {
            return null;
        }

        // treat invalid slots as empty
        if (string.IsNullOrEmpty(slot.saveDate))
        {
            return null;
        }

        return slot;
    }

    // =========================================================
    // LOAD GAME FROM SLOT (MAIN ENTRY POINT)
    // =========================================================
    public static void LoadGameFromSlot(int index)
    {
        SaveSlot slot = LoadSlot(index);

        if (slot == null)
        {
            return;
        }

        PendingLoad = slot;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // =========================================================
    // LOAD LATEST SAVE
    // =========================================================
    public static void LoadLatestSave()
    {
        List<(int index, SaveSlot slot)> slots = GetAllSlots();

        if (slots.Count == 0)
        {
            return;
        }

        LoadGameFromSlot(slots[0].index);
    }

    // =========================================================
    // GET ALL SLOTS
    // =========================================================
    public static List<(int index, SaveSlot slot)> GetAllSlots()
    {
        List<(int index, SaveSlot slot)> list =
            new List<(int index, SaveSlot slot)>();

        for (int i = 0; i < MAX_SLOTS; i++)
        {
            SaveSlot slot = LoadSlot(i);

            if (slot != null)
            {
                list.Add((i, slot));
            }
        }

        list.Sort((a, b) =>
            string.Compare(b.slot.saveDate, a.slot.saveDate)
        );

        return list;
    }

    // =========================================================
    // HELPERS
    // =========================================================
    private static string CaptureScreenshot(int slotIndex)
    {
        string path =
            Application.persistentDataPath +
            "/save_" + slotIndex + ".png";

        ScreenCapture.CaptureScreenshot(path);

        return path;
    }
    public static int GetSlotCount()
    {
        return MAX_SLOTS;
    }
}