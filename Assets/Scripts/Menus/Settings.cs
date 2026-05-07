using UnityEngine;

public static class Settings
{
    // =========================================================
    // AUDIO
    // =========================================================
    public static float MasterVolume = 1f;
    public static float MusicVolume = 1f;
    public static float SFXVolume = 1f;

    // =========================================================
    // GAMEPLAY
    // =========================================================
    public static bool AutoAdvance = false;
    public static int TextSpeed = 1;
    public static bool HintsEnabled = true;

    // =========================================================
    // GRAPHICS
    // =========================================================
    public static float ShadowLevel = 50f;
    public static float Brightness = 50f;

    public static int ColorblindMode = 0;
    public static float ColorblindAmount = 0f;

    public static int ResolutionIndex = 0;
    public static int QualityLevel = 1;
    public static bool Fullscreen = true;

    // =========================================================
    // SAVE
    // =========================================================
    public static void Save()
    {
        // AUDIO
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);

        // GAMEPLAY
        if (AutoAdvance)
        {
            PlayerPrefs.SetInt("AutoAdvance", 1);
        }
        else
        {
            PlayerPrefs.SetInt("AutoAdvance", 0);
        }

        PlayerPrefs.SetInt("TextSpeed", TextSpeed);

        if (HintsEnabled)
        {
            PlayerPrefs.SetInt("HintsEnabled", 1);
        }
        else
        {
            PlayerPrefs.SetInt("HintsEnabled", 0);
        }

        // GRAPHICS
        PlayerPrefs.SetFloat("ShadowLevel", ShadowLevel);
        PlayerPrefs.SetFloat("Brightness", Brightness);

        PlayerPrefs.SetInt("ColorblindMode", ColorblindMode);
        PlayerPrefs.SetFloat("ColorblindAmount", ColorblindAmount);

        PlayerPrefs.SetInt("ResolutionIndex", ResolutionIndex);
        PlayerPrefs.SetInt("QualityLevel", QualityLevel);

        if (Fullscreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }

        PlayerPrefs.Save();
    }

    // =========================================================
    // LOAD
    // =========================================================
    public static void Load()
    {
        // AUDIO
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // GAMEPLAY
        if (PlayerPrefs.GetInt("AutoAdvance", 0) == 1)
        {
            AutoAdvance = true;
        }
        else
        {
            AutoAdvance = false;
        }

        TextSpeed = PlayerPrefs.GetInt("TextSpeed", 1);

        if (PlayerPrefs.GetInt("HintsEnabled", 1) == 1)
        {
            HintsEnabled = true;
        }
        else
        {
            HintsEnabled = false;
        }

        // GRAPHICS
        ShadowLevel = PlayerPrefs.GetFloat("ShadowLevel", 50f);
        Brightness = PlayerPrefs.GetFloat("Brightness", 50f);

        ColorblindMode = PlayerPrefs.GetInt("ColorblindMode", 0);
        ColorblindAmount = PlayerPrefs.GetFloat("ColorblindAmount", 0f);

        ResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        QualityLevel = PlayerPrefs.GetInt("QualityLevel", 1);

        if (PlayerPrefs.GetInt("Fullscreen", 1) == 1)
        {
            Fullscreen = true;
        }
        else
        {
            Fullscreen = false;
        }

        // SAFETY
        ResolutionIndex = Mathf.Clamp(
            ResolutionIndex,
            0,
            Screen.resolutions.Length - 1
        );
    }
}