using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle[] qualityToggles;
    [SerializeField] private Slider shadowSlider;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Dropdown colorblindDropdown;
    [SerializeField] private TextMeshProUGUI shadowText;
    [SerializeField] private TextMeshProUGUI brightnessText;
    [SerializeField] private TextMeshProUGUI colorblindText;

    [SerializeField] private Slider colorblindSlider;

    private Resolution[] allResolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    // =========================================================
    // INIT
    // =========================================================
    void OnEnable()
    {
        colorblindDropdown.onValueChanged.RemoveAllListeners();
        colorblindDropdown.onValueChanged.AddListener(OnColorblindChanged);
        shadowSlider.onValueChanged.AddListener(UpdateShadowText);
        brightnessSlider.onValueChanged.AddListener(UpdateBrightnessText);
        colorblindSlider.onValueChanged.AddListener(UpdateColorblindText);

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
    void UpdateShadowText(float value)
    {
        shadowText.text = Mathf.RoundToInt(value).ToString();
    }

    void UpdateBrightnessText(float value)
    {
        brightnessText.text = Mathf.RoundToInt(value).ToString();
    }

    void UpdateColorblindText(float value)
    {
        colorblindText.text = Mathf.RoundToInt(value).ToString();
    }

    // =========================================================
    // RESOLUTION SETUP (FIXED)
    // =========================================================
    void SetupResolutions()
    {
        allResolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        uniqueResolutions.Clear();

        List<string> options = new List<string>();
        HashSet<string> seen = new HashSet<string>();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            Resolution res = allResolutions[i];
            string option = res.width + "x" + res.height;

            if (!seen.Contains(option))
            {
                seen.Add(option);
                uniqueResolutions.Add(res);
                options.Add(option);
            }
        }

        resolutionDropdown.AddOptions(options);

        // Clamp saved index (prevents crash)
        int savedIndex = Mathf.Clamp(Settings.ResolutionIndex, 0, uniqueResolutions.Count - 1);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // =========================================================
    // APPLY SETTINGS (FIXED)
    // =========================================================
    public void Apply()
    {
        int index = resolutionDropdown.value;

        if (index < 0 || index >= uniqueResolutions.Count)
            return;

        Resolution res = uniqueResolutions[index];

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
    }

    // =========================================================
    // REFRESH UI
    // =========================================================
    public void RefreshUI()
    {
        UpdateShadowText(shadowSlider.value);
        UpdateBrightnessText(brightnessSlider.value);
        UpdateColorblindText(colorblindSlider.value);
        fullscreenToggle.isOn = Settings.Fullscreen;

        int index = Mathf.Clamp(Settings.ResolutionIndex, 0, resolutionDropdown.options.Count - 1);
        resolutionDropdown.value = index;
        resolutionDropdown.RefreshShownValue();

        for (int i = 0; i < qualityToggles.Length; i++)
        {
            qualityToggles[i].isOn = (i == Settings.QualityLevel);
        }

        shadowSlider.value = Settings.ShadowLevel;
        brightnessSlider.value = Settings.Brightness;

        colorblindDropdown.value = Settings.ColorblindMode;
    }
}