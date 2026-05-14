using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private const int MAX_SLOTS = 10;

    // =========================================================
    // SAVE GAME
    // =========================================================
    public static void SaveGame()
    {
        SaveSlot slot = BuildSaveSlot();

        int slotIndex = GetNextAvailableSlot();

        PlayerPrefs.SetString("SaveSlot_" + slotIndex, JsonUtility.ToJson(slot));
        PlayerPrefs.SetInt("HasSave", 1);
        PlayerPrefs.Save();
    }

    // =========================================================
    // BUILD SAVE DATA
    // =========================================================
    private static SaveSlot BuildSaveSlot()
    {
        GameObject player = GameObject.FindWithTag("Player");

        SaveSlot slot = new SaveSlot();

        if (player != null)
        {
            Vector3 pos = player.transform.position;

            slot.playerX = pos.x;
            slot.playerY = pos.y;
            slot.playerZ = pos.z;

            slot.regionID =
                RegionMapManager.Instance.GetRegionFromPosition(pos);
        }

        // SAVE DATE
        slot.saveDate =
            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // SAVE PLAYTIME
        if (PlayTimeManager.Instance != null)
        {
            slot.playTime =
                PlayTimeManager.Instance.GetPlayTime();
        }

        return slot;
    }

    // =========================================================
    // LOAD SINGLE SLOT
    // =========================================================
    public static SaveSlot LoadSlot(int index)
    {
        string json = PlayerPrefs.GetString("SaveSlot_" + index, "");

        if (string.IsNullOrEmpty(json))
            return null;

        return JsonUtility.FromJson<SaveSlot>(json);
    }

    // =========================================================
    // GET ALL SLOTS
    // =========================================================
    public static List<(int index, SaveSlot slot)> GetAllSlots()
    {
        List<(int index, SaveSlot slot)> list = new();

        for (int i = 0; i < MAX_SLOTS; i++)
        {
            SaveSlot slot = LoadSlot(i);

            if (slot != null)
                list.Add((i, slot));
        }

        // newest first
        list.Sort((a, b) =>
            string.Compare(b.slot.saveDate, a.slot.saveDate)
        );

        return list;
    }

    // =========================================================
    // LOAD FROM SLOT
    // =========================================================
    public static void LoadGameFromSlot(int index)
    {
        SaveSlot slot = LoadSlot(index);

        if (slot == null)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // IMPORTANT:
        // Position restoration happens AFTER scene load
        // You will hook this in a PlayerSpawnManager or GameManager
        PendingLoad = slot;
    }

    // =========================================================
    // LOAD LATEST SAVE (FOR CONTINUE BUTTON)
    // =========================================================
    public static void LoadLatestSave()
    {
        var slots = GetAllSlots();

        if (slots.Count == 0)
            return;

        LoadGameFromSlot(slots[0].index);
    }

    // =========================================================
    // PENDING LOAD SYSTEM (POST-SCENE RESTORE)
    // =========================================================
    public static SaveSlot PendingLoad;

    public static bool HasPendingLoad()
    {
        return PendingLoad != null;
    }

    public static SaveSlot ConsumePendingLoad()
    {
        SaveSlot temp = PendingLoad;
        PendingLoad = null;
        return temp;
    }

    // =========================================================
    // SLOT FINDER
    // =========================================================
    private static int GetNextAvailableSlot()
    {
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            if (!PlayerPrefs.HasKey("SaveSlot_" + i))
                return i;
        }

        return 0;
    }
}