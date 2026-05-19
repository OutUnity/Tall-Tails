using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private const int MAX_SLOTS = 10;

    public static SaveSlot PendingLoad;
    public static string gameplaySceneName = "GameScene";

    // =====================================================
    // SAVE GAME
    // =====================================================
    public static void SaveGame(int slotIndex)
    {
        SaveSlot slot = BuildSaveSlot(slotIndex);

        string path = GetScreenshotPath(slotIndex);
        slot.screenshotPath = path;

        ScreenCapture.CaptureScreenshot(path);

        PlayerPrefs.SetString("SaveSlot_" + slotIndex, JsonUtility.ToJson(slot));
        PlayerPrefs.Save();
    }

    // =====================================================
    // BUILD SAVE DATA
    // =====================================================
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
                slot.regionID = RegionMapManager.Instance.GetRegionFromPosition(pos);
            }
        }

        slot.saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (PlayTimeManager.Instance != null)
        {
            slot.playTime = PlayTimeManager.Instance.GetPlayTime();
        }

        return slot;
    }

    // =====================================================
    // LOAD SLOT
    // =====================================================
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

        if (string.IsNullOrEmpty(slot.saveDate))
        {
            return null;
        }

        return slot;
    }

    // =====================================================
    // LOAD GAME (MAIN ENTRY)
    // =====================================================
    public static void LoadGameFromSlot(int index)
    {
        SaveSlot slot = LoadSlot(index);

        if (slot == null)
        {
            Debug.LogWarning("Tried to load empty slot: " + index);
            return;
        }

        PendingLoad = slot;

        if (LoadingUI.Instance != null)
        {
            LoadingUI.Instance.Show();
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    // =====================================================
    // APPLY LATEST SAVE
    // =====================================================
    public static void LoadLatestSave()
    {
        List<(int index, SaveSlot slot)> slots = GetAllSlots();

        if (slots.Count == 0)
        {
            return;
        }

        LoadGameFromSlot(slots[0].index);
    }

    // =====================================================
    // GET SLOTS
    // =====================================================
    public static List<(int index, SaveSlot slot)> GetAllSlots()
    {
        List<(int index, SaveSlot slot)> list = new List<(int index, SaveSlot slot)>();

        for (int i = 0; i < MAX_SLOTS; i++)
        {
            SaveSlot slot = LoadSlot(i);

            if (slot != null)
            {
                list.Add((i, slot));
            }
        }

        list.Sort((a, b) =>
        {
            return string.Compare(b.slot.saveDate, a.slot.saveDate);
        });

        return list;
    }

    // =====================================================
    // SLOT COUNT
    // =====================================================
    public static int GetSlotCount()
    {
        return MAX_SLOTS;
    }

    // =====================================================
    // DELETE
    // =====================================================
    public static void DeleteSlot(int index)
    {
        PlayerPrefs.DeleteKey("SaveSlot_" + index);
        PlayerPrefs.Save();
    }

    public static void DeleteAllSaves()
    {
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            PlayerPrefs.DeleteKey("SaveSlot_" + i);
        }

        PlayerPrefs.Save();
    }

    // =====================================================
    // SCREENSHOT PATH
    // =====================================================
    private static string GetScreenshotPath(int slotIndex)
    {
        return Application.persistentDataPath + "/save_" + slotIndex + ".png";
    }

    // =====================================================
    // DEBUG CHECK
    // =====================================================
    public static bool HasAnySave()
    {
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            SaveSlot slot = LoadSlot(i);

            if (slot != null)
            {
                return true;
            }
        }

        return false;
    }
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

}