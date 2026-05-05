using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    /*[Header("Text")]
    [SerializeField] private TMP_Text masterText;
    [SerializeField] private TMP_Text musicText;
    [SerializeField] private TMP_Text sfxText;
    */
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Accessibility")]
    [SerializeField] private Toggle autoAdvanceToggle;
    [SerializeField] private Toggle[] textSpeedToggles; // 0=slow,1=med,2=fast
    [SerializeField] private Toggle hintsToggle;

    void Start()
    {
        RefreshUI();
    }

    void Update()
    {
        //UpdateText();
    }

    // ---------------------------
    // APPLY
    // ---------------------------
    public void Apply()
    {
        Settings.MasterVolume = masterSlider.value;
        Settings.MusicVolume = musicSlider.value;
        Settings.SFXVolume = sfxSlider.value;

        mixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
        mixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);

        Settings.AutoAdvance = autoAdvanceToggle.isOn;
        Settings.HintsEnabled = hintsToggle.isOn;

        for (int i = 0; i < textSpeedToggles.Length; i++)
        {
            if (textSpeedToggles[i].isOn)
            {
                Settings.TextSpeed = i;
                break;
            }
        }
    }

    // ---------------------------
    // UI REFRESH
    // ---------------------------
    public void RefreshUI()
    {
        masterSlider.value = Settings.MasterVolume;
        musicSlider.value = Settings.MusicVolume;
        sfxSlider.value = Settings.SFXVolume;

        autoAdvanceToggle.isOn = Settings.AutoAdvance;
        hintsToggle.isOn = Settings.HintsEnabled;

        for (int i = 0; i < textSpeedToggles.Length; i++)
        {
            textSpeedToggles[i].isOn = (i == Settings.TextSpeed);
        }

        //UpdateText();
    }

    // ---------------------------
    // LIVE TEXT UPDATE
    // ---------------------------
    /*void UpdateText()
    {
        masterText.text = Mathf.RoundToInt(masterSlider.value * 100) + "%";
        musicText.text = Mathf.RoundToInt(musicSlider.value * 100) + "%";
        sfxText.text = Mathf.RoundToInt(sfxSlider.value * 100) + "%";
    }*/
}