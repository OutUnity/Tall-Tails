using System;
using System.Collections;
using UnityEngine;
using System.IO;

public class ScreenshotSaver : MonoBehaviour
{
    public static ScreenshotSaver Instance;

    // =====================================================
    // TEMP STORAGE (FOR PAUSE MENU PREVIEW)
    // =====================================================
    public Texture2D LastFrame { get; private set; }

    // =====================================================
    // SINGLETON
    // =====================================================
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =====================================================
    // GET OR CREATE
    // =====================================================
    public static ScreenshotSaver Get()
    {
        if (Instance != null) return Instance;

        GameObject obj = new GameObject("ScreenshotSaver");
        Instance = obj.AddComponent<ScreenshotSaver>();
        DontDestroyOnLoad(obj);

        return Instance;
    }

    // =====================================================
    // STEP 1: CAPTURE TEMP FRAME (PAUSE MENU PREVIEW)
    // =====================================================
    public IEnumerator CaptureTemp()
    {
        yield return new WaitForEndOfFrame();

        if (LastFrame != null)
        {
            Destroy(LastFrame);
        }

        LastFrame = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        LastFrame.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        LastFrame.Apply();
    }

    // =====================================================
    // STEP 2: SAVE TEMP FRAME TO DISK (FINAL SAVE)
    // =====================================================
    public string SaveTempToDisk(int slotIndex)
    {
        if (LastFrame == null)
        {
            Debug.LogError("No screenshot available in temp buffer!");
            return null;
        }

        string path =
            Application.persistentDataPath +
            "/save_" + slotIndex + ".png";

        try
        {
            File.WriteAllBytes(path, LastFrame.EncodeToPNG());
        }
        catch (Exception e)
        {
            Debug.LogError("Screenshot save failed: " + e.Message);
            return null;
        }

        return path;
    }

    // =====================================================
    // OPTIONAL: OLD SYSTEM (KEEP FOR COMPATIBILITY)
    // =====================================================
    public void Capture(int slotIndex, Action<string> callback)
    {
        StartCoroutine(OldCaptureRoutine(slotIndex, callback));
    }

    private IEnumerator OldCaptureRoutine(int slotIndex, Action<string> callback)
    {
        yield return new WaitForEndOfFrame();

        string path =
            Application.persistentDataPath +
            "/save_" + slotIndex + ".png";

        ScreenCapture.CaptureScreenshot(path);

        yield return new WaitForSeconds(0.2f);

        callback?.Invoke(path);
    }
}