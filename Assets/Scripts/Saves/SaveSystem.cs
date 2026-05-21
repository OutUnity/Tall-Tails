using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private const int MAX_SLOTS = 10;

    public static SaveSlot PendingLoad;
    public static string gameplaySceneName = "GameScene";

    public static System.Action OnBeforeScreenshot;

    // =====================================================
    // SAVE GAME (ENTRY POINT)
    // =====================================================
    public static void SaveGame(int slotIndex, System.Action onComplete = null)
    {
        if (CoroutineRunner.Instance == null)
        {
            Debug.LogError("CoroutineRunner missing from scene!");
            return;
        }

        CoroutineRunner.Instance.StartCoroutine(SaveRoutine(slotIndex, onComplete));
    }

    // =====================================================
    // SAVE ROUTINE (NOW USES ScreenshotSaver)
    // =====================================================
    private static IEnumerator SaveRoutine(int slotIndex, System.Action onComplete)
    {
        Debug.Log("SaveRoutine running...");

        // Wait for UI/layout to settle
        yield return new WaitForEndOfFrame();

        SaveSlot slot = BuildSaveSlot(slotIndex);

        if (slot == null)
        {
            Debug.LogError("Save aborted: slot was null.");
            yield break;
        }

        // TEMP: hide UI before screenshot
        OnBeforeScreenshot?.Invoke();

        string screenshotPath = null;

        bool done = false;

        // =====================================================
        // CALL SCREENSHOT SYSTEM (IMPORTANT FIX)
        // =====================================================
        ScreenshotSaver.Instance.Capture(slotIndex, (path) =>
        {
            screenshotPath = path;
            done = true;
        });

        // Wait until screenshot is actually written
        while (!done)
            yield return null;

        slot.screenshotPath = screenshotPath;

        PlayerPrefs.SetString(
            "SaveSlot_" + slotIndex,
            JsonUtility.ToJson(slot)
        );

        PlayerPrefs.Save();

        Debug.Log("SAVE COMPLETE");

        onComplete?.Invoke();
    }

    // =====================================================
    // LOAD GAME FROM SLOT
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

        GameLoader loader = Object.FindFirstObjectByType<GameLoader>();

        if (loader != null)
        {
            loader.LoadGame();
        }
        else
        {
            Debug.LogError("GameLoader not found in scene.");
        }
    }

    // =====================================================
    // LOAD LATEST SAVE
    // =====================================================
    public static void LoadLatestSave()
    {
        List<(int index, SaveSlot slot)> slots = GetAllSlots();

        if (slots.Count == 0)
        {
            Debug.LogWarning("No saves found.");
            return;
        }

        LoadGameFromSlot(slots[0].index);
    }

    // =====================================================
    // SLOT LOAD
    // =====================================================
    public static SaveSlot LoadSlot(int index)
    {
        string json = PlayerPrefs.GetString("SaveSlot_" + index, "");

        if (string.IsNullOrEmpty(json))
            return null;

        SaveSlot slot = JsonUtility.FromJson<SaveSlot>(json);

        if (slot == null || string.IsNullOrEmpty(slot.saveDate))
            return null;

        return slot;
    }

    // =====================================================
    // GET ALL SLOTS
    // =====================================================
    public static List<(int index, SaveSlot slot)> GetAllSlots()
    {
        List<(int index, SaveSlot slot)> list =
            new List<(int index, SaveSlot slot)>();

        for (int i = 0; i < MAX_SLOTS; i++)
        {
            SaveSlot slot = LoadSlot(i);

            if (slot != null)
                list.Add((i, slot));
        }

        list.Sort((a, b) =>
            string.Compare(b.slot.saveDate, a.slot.saveDate)
        );

        return list;
    }

    // =====================================================
    // CHECK SAVES
    // =====================================================
    public static bool HasAnySave()
    {
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            if (LoadSlot(i) != null)
                return true;
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

    public static int GetSlotCount()
    {
        return MAX_SLOTS;
    }

    // =====================================================
    // BUILD SAVE DATA
    // =====================================================
    private static SaveSlot BuildSaveSlot(int slotIndex)
    {
        Debug.Log("Building Save Slot...");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("SAVE FAILED: Player not found in scene.");
            return null;
        }

        SaveSlot slot = new SaveSlot();

        Vector3 pos = player.transform.position;

        Debug.Log("Player found at: " + pos);

        slot.playerX = pos.x;
        slot.playerY = pos.y;
        slot.playerZ = pos.z;

        if (RegionMapManager.Instance != null)
        {
            slot.regionID =
                RegionMapManager.Instance.GetRegionFromPosition(pos);
        }

        slot.saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        slot.sceneName = gameplaySceneName;

        if (PlayTimeManager.Instance != null)
        {
            slot.playTime = PlayTimeManager.Instance.GetPlayTime();
        }

        return slot;
    }
}