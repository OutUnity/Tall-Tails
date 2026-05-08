using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI masterText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI sfxText;

    void OnEnable()
    {
        RefreshUI();

        masterSlider.onValueChanged.AddListener(UpdateMaster);
        musicSlider.onValueChanged.AddListener(UpdateMusic);
        sfxSlider.onValueChanged.AddListener(UpdateSFX);
    }

    // =========================================================
    // REFRESH
    // =========================================================

    public void RefreshUI()
    {
        masterSlider.SetValueWithoutNotify(Settings.MasterVolume);
        musicSlider.SetValueWithoutNotify(Settings.MusicVolume);
        sfxSlider.SetValueWithoutNotify(Settings.SFXVolume);

        UpdateMasterText(Settings.MasterVolume);
        UpdateMusicText(Settings.MusicVolume);
        UpdateSFXText(Settings.SFXVolume);
    }

    // =========================================================
    // MASTER
    // =========================================================

    void UpdateMaster(float value)
    {
        Settings.MasterVolume = value;

        audioMixer.SetFloat(
            "MasterVolume",
            Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f
        );

        UpdateMasterText(value);
    }

    // =========================================================
    // MUSIC
    // =========================================================

    void UpdateMusic(float value)
    {
        Settings.MusicVolume = value;

        audioMixer.SetFloat(
            "MusicVolume",
            Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f
        );

        UpdateMusicText(value);
    }

    // =========================================================
    // SFX
    // =========================================================

    void UpdateSFX(float value)
    {
        Settings.SFXVolume = value;

        audioMixer.SetFloat(
            "SFXVolume",
            Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f
        );

        UpdateSFXText(value);
    }

    // =========================================================
    // TEXT
    // =========================================================

    void UpdateMasterText(float value)
    {
        masterText.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    void UpdateMusicText(float value)
    {
        musicText.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    void UpdateSFXText(float value)
    {
        sfxText.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    // =========================================================
    // APPLY
    // =========================================================

    public void Apply()
    {
        Settings.Save();
    }
}