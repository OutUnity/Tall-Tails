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
    [SerializeField] private Slider colorblindSlider;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI shadowText;
    [SerializeField] private TextMeshProUGUI brightnessText;
    [SerializeField] private TextMeshProUGUI colorblindText;

    private Resolution[] allResolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    // =========================================================
    // INIT
    // =========================================================
    void OnEnable()
    {
        SetupListeners();
        SetupResolutions();
        RefreshUI();
    }

    void SetupListeners()
    {
        colorblindDropdown.onValueChanged.RemoveAllListeners();
        colorblindSlider.onValueChanged.RemoveAllListeners();
        shadowSlider.onValueChanged.RemoveAllListeners();
        brightnessSlider.onValueChanged.RemoveAllListeners();

        colorblindDropdown.onValueChanged.AddListener(OnColorblindChanged);
        colorblindSlider.onValueChanged.AddListener(OnColorblindAmountChanged);
        shadowSlider.onValueChanged.AddListener(OnShadowChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
    }

    // =========================================================
    // RESOLUTIONS
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
            string option = res.width + " x " + res.height;

            if (!seen.Contains(option))
            {
                seen.Add(option);
                uniqueResolutions.Add(res);
                options.Add(option);
            }
        }

        resolutionDropdown.AddOptions(options);

        int index = Mathf.Clamp(Settings.ResolutionIndex, 0, uniqueResolutions.Count - 1);
        resolutionDropdown.value = index;
        resolutionDropdown.RefreshShownValue();
    }

    // =========================================================
    // APPLY (CALL THIS FROM BUTTON)
    // =========================================================
    public void Apply()
    {
        ApplyResolution();
        ApplyQuality();
        ApplyShadows();
        ApplyBrightness();
        ApplyColorblind();
    }

    void ApplyResolution()
    {
        int index = Mathf.Clamp(resolutionDropdown.value, 0, uniqueResolutions.Count - 1);
        Resolution res = uniqueResolutions[index];

        Screen.SetResolution(res.width, res.height, fullscreenToggle.isOn);

        Settings.ResolutionIndex = index;
        Settings.Fullscreen = fullscreenToggle.isOn;
    }

    void ApplyQuality()
    {
        for (int i = 0; i < qualityToggles.Length; i++)
        {
            if (qualityToggles[i].isOn)
            {
                QualitySettings.SetQualityLevel(i);
                Settings.QualityLevel = i;
                break;
            }
        }
    }

    void ApplyShadows()
    {
        float value = shadowSlider.value;

        Settings.ShadowLevel = value;

        // 0-100 slider → Unity shadow quality
        if (value < 33)
        {
            QualitySettings.shadowResolution = ShadowResolution.Low;
        }
        else if (value < 66)
        {
            QualitySettings.shadowResolution = ShadowResolution.Medium;
        }
        else
        {
            QualitySettings.shadowResolution = ShadowResolution.High;
        }
    }

    void ApplyBrightness()
    {
        Settings.Brightness = brightnessSlider.value;
    }

    void ApplyColorblind()
    {
        Settings.ColorblindMode = colorblindDropdown.value;
        Settings.ColorblindAmount = colorblindSlider.value;
    }

    // =========================================================
    // UI EVENTS
    // =========================================================
    void OnColorblindChanged(int index)
    {
        Settings.ColorblindMode = index;
    }

    void OnColorblindAmountChanged(float value)
    {
        Settings.ColorblindAmount = value;
        UpdateColorblindText(value);
    }

    void OnShadowChanged(float value)
    {
        UpdateShadowText(value);
    }

    void OnBrightnessChanged(float value)
    {
        UpdateBrightnessText(value);
    }

    // =========================================================
    // TEXT UPDATES
    // =========================================================
    void UpdateShadowText(float value)
    {
        if (shadowText != null)
            shadowText.text = Mathf.RoundToInt(value).ToString();
    }

    void UpdateBrightnessText(float value)
    {
        if (brightnessText != null)
            brightnessText.text = Mathf.RoundToInt(value).ToString();
    }

    void UpdateColorblindText(float value)
    {
        if (colorblindText != null)
            colorblindText.text = Mathf.RoundToInt(value).ToString();
    }

    // =========================================================
    // REFRESH UI FROM SETTINGS
    // =========================================================
    public void RefreshUI()
    {
        fullscreenToggle.SetIsOnWithoutNotify(Settings.Fullscreen);

        resolutionDropdown.value = Mathf.Clamp(
            Settings.ResolutionIndex,
            0,
            uniqueResolutions.Count - 1
        );
        resolutionDropdown.RefreshShownValue();

        for (int i = 0; i < qualityToggles.Length; i++)
        {
            qualityToggles[i].SetIsOnWithoutNotify(i == Settings.QualityLevel);
        }

        shadowSlider.SetValueWithoutNotify(Mathf.Clamp(Settings.ShadowLevel, 0f, 100f));

        brightnessSlider.SetValueWithoutNotify(Mathf.Clamp(Settings.Brightness, 0f, 100f));

        colorblindDropdown.SetValueWithoutNotify(Settings.ColorblindMode);
        colorblindSlider.SetValueWithoutNotify(Settings.ColorblindAmount);

        UpdateShadowText(shadowSlider.value);
        UpdateBrightnessText(brightnessSlider.value);
        UpdateColorblindText(colorblindSlider.value);
    }
}