using UnityEngine;

public static class Settings
{
    public static float MasterVolume = 1f;
    public static float MusicVolume = 1f;
    public static float SFXVolume = 1f;

    public static bool AutoAdvance = false;
    public static int TextSpeed = 1;
    public static bool HintsEnabled = true;

    public static float ShadowLevel = .5f;
    public static float Brightness = .5f;

    public static int ColorblindMode = 0;
    public static float ColorblindAmount = 0f;

    public static int ResolutionIndex = 0;
    public static int QualityLevel = 1;
    public static bool Fullscreen = true;

    // ---------------------------
    // SAVE
    // ---------------------------
    public static void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);

        PlayerPrefs.SetInt("AutoAdvance", AutoAdvance ? 1 : 0);
        PlayerPrefs.SetInt("TextSpeed", TextSpeed);
        PlayerPrefs.SetInt("HintsEnabled", HintsEnabled ? 1 : 0);

        PlayerPrefs.SetFloat("ShadowLevel", ShadowLevel);
        PlayerPrefs.SetFloat("Brightness", Brightness);

        PlayerPrefs.SetInt("ColorblindMode", ColorblindMode);
        PlayerPrefs.SetFloat("ColorblindAmount", ColorblindAmount);

        PlayerPrefs.SetInt("ResolutionIndex", ResolutionIndex);
        PlayerPrefs.SetInt("QualityLevel", QualityLevel);
        PlayerPrefs.SetInt("Fullscreen", Fullscreen ? 1 : 0);

        PlayerPrefs.Save();
    }

    // ---------------------------
    // LOAD
    // ---------------------------
    public static void Load()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        AutoAdvance = PlayerPrefs.GetInt("AutoAdvance", 0) == 1;
        TextSpeed = PlayerPrefs.GetInt("TextSpeed", 1);
        HintsEnabled = PlayerPrefs.GetInt("HintsEnabled", 1) == 1;

        ShadowLevel = PlayerPrefs.GetFloat("ShadowLevel", .5f);
        Brightness = PlayerPrefs.GetFloat("Brightness", .5f);

        ColorblindMode = PlayerPrefs.GetInt("ColorblindMode", 0);
        ColorblindAmount = PlayerPrefs.GetFloat("ColorblindAmount", 0f);

        ResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        QualityLevel = PlayerPrefs.GetInt("QualityLevel", 1);
        Fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }
}