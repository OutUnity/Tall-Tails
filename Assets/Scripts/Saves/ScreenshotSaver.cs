using UnityEngine;

public class ScreenshotSaver : MonoBehaviour
{
    public static ScreenshotSaver Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Capture(int slotIndex, System.Action<string> callback)
    {
        StartCoroutine(CaptureRoutine(slotIndex, callback));
    }

    private System.Collections.IEnumerator CaptureRoutine(int slotIndex, System.Action<string> callback)
    {
        yield return new WaitForEndOfFrame();

        string path = Application.persistentDataPath + "/save_" + slotIndex + ".png";

        ScreenCapture.CaptureScreenshot(path);

        yield return new WaitForSeconds(0.2f);

        callback?.Invoke(path);
    }
}
