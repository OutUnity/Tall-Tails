using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsSettingsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle[] qualityToggles;
    [SerializeField] private Slider shadowSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Dropdown colorblindDropdown;
    //[SerializeField] private Slider colorblindAmountSlider;

    private Resolution[] resolutions;

    // =========================================================
    // INIT (FIXED TIMING ISSUE)
    // =========================================================
    void OnEnable()
    {
        // prevent duplicate listeners
        colorblindDropdown.onValueChanged.RemoveAllListeners();
        //colorblindAmountSlider.onValueChanged.RemoveAllListeners();

        colorblindDropdown.onValueChanged.AddListener(OnColorblindChanged);
        //colorblindAmountSlider.onValueChanged.AddListener(OnColorblindAmountChanged);

        // delay ensures UI is fully ready (IMPORTANT FIX)
        Invoke(nameof(InitUI), 0.01f);
    }

    void InitUI()
    {
        SetupResolutions();
        RefreshUI();
    }

    // =========================================================
    // LISTENERS
    // =========================================================
    void OnColorblindChanged(int index)
    {
        Settings.ColorblindMode = index;
    }

    void OnColorblindAmountChanged(float value)
    {
        Settings.ColorblindAmount = value;
    }

    // =========================================================
    // RESOLUTION SETUP
    // =========================================================
    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + "x" + resolutions[i].height);
        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = Settings.ResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // =========================================================
    // APPLY SETTINGS
    // =========================================================
    public void Apply()
    {
        // Resolution
        int index = resolutionDropdown.value;
        Resolution res = resolutions[index];

        Screen.SetResolution(res.width, res.height, fullscreenToggle.isOn);

        Settings.ResolutionIndex = index;
        Settings.Fullscreen = fullscreenToggle.isOn;

        // Quality
        for (int i = 0; i < qualityToggles.Length; i++)
        {
            if (qualityToggles[i].isOn)
            {
                QualitySettings.SetQualityLevel(i);
                Settings.QualityLevel = i;
                break;
            }
        }

        // Shadows
        int shadowIndex = Mathf.RoundToInt(shadowSlider.value);
        QualitySettings.shadowResolution = (ShadowResolution)shadowIndex;
        Settings.ShadowLevel = shadowIndex;

        // Brightness
        Settings.Brightness = brightnessSlider.value;

        // Colorblind
        Settings.ColorblindMode = colorblindDropdown.value;
       // Settings.ColorblindAmount = colorblindAmountSlider.value;
    }

    // =========================================================
    // REFRESH UI FROM SETTINGS
    // =========================================================
    public void RefreshUI()
    {
        fullscreenToggle.isOn = Settings.Fullscreen;

        resolutionDropdown.value = Settings.ResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Quality toggles
        for (int i = 0; i < qualityToggles.Length; i++)
        {
            qualityToggles[i].isOn = (i == Settings.QualityLevel);
        }

        shadowSlider.value = Settings.ShadowLevel;
        brightnessSlider.value = Settings.Brightness;

        colorblindDropdown.value = Settings.ColorblindMode;
        //colorblindAmountSlider.value = Settings.ColorblindAmount;
    }
}