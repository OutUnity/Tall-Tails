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
        canvasGroup.alpha = 0;
    }

    public void ShowRegion(int regionID)
    {
        string regionName = GetRegionName(regionID);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(regionName));
    }

    IEnumerator ShowRoutine(string regionName)
    {
        regionText.text = regionName;

        // Fade in
        yield return Fade(0, 1);

        yield return new WaitForSeconds(2f);

        // Fade out
        yield return Fade(1, 0);
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

    string GetRegionName(int id)
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
