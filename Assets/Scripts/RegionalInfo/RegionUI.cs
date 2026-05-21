using UnityEngine;
using TMPro;
using System.Collections;

public class RegionUI : MonoBehaviour
{
    public static RegionUI Instance;

    public TextMeshProUGUI regionText;
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.5f;
    public float displayTime = 2f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        Instance = this;

        if (canvasGroup != null)
            canvasGroup.alpha = 0;
    }

    public void ShowRegion(int regionID)
    {
        //Debug.Log("ShowRegion called: " + regionID);

        string regionName = GetRegionName(regionID);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(regionName));
    }

    IEnumerator ShowRoutine(string regionName)
    {
        if (regionText == null || canvasGroup == null)
        {
            Debug.LogError("RegionUI references not set!");
            yield break;
        }

        regionText.text = regionName;

        // Fade in
        yield return StartCoroutine(Fade(0, 1));

        // Stay visible
        yield return new WaitForSeconds(displayTime);

        // Fade out
        yield return StartCoroutine(Fade(1, 0));
    }

    IEnumerator Fade(float start, float end)
    {
        float time = 0;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end;
    }

    public string GetRegionName(int id)
    {
        switch (id)
        {
            case 1: return "Whispering Grove";
            case 2: return "Crystal Falls";
            case 3: return "Sunlit Meadow";
            case 4: return "Azure Ridge";
            case 5: return "Mosswood Hollow";
            case 6: return "Golden Highlands";
            case 7: return "Starlight Basin";
            default: return "Unknown";
        }
    }
}