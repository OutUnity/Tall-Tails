using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RegionMusic
{
    public int regionID;
    public List<AudioClip> playlist = new List<AudioClip>();
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Main Menu Music")]
    public List<AudioClip> mainMenuPlaylist = new List<AudioClip>();

    [Header("Region Music")]
    public List<RegionMusic> regions = new List<RegionMusic>();

    [Header("Audio Settings")]
    public AudioSource musicSource;
    public float fadeDuration = 2f;
    [Range(0f, 1f)] public float volume = 0.5f;

    private List<AudioClip> currentPlaylist;
    private AudioClip lastClip;
    private Coroutine musicRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = false;
        musicSource.spatialBlend = 0f; // always 2D
        musicSource.volume = volume;
    }

    // 🎵 PLAY MAIN MENU
    public void PlayMainMenu()
    {
        StartPlaylist(mainMenuPlaylist);
    }

    // 🌍 PLAY REGION
    public void SetRegion(int regionID)
    {
        RegionMusic region = regions.Find(r => r.regionID == regionID);

        if (region == null || region.playlist.Count == 0)
        {
            Debug.LogWarning("No music found for region " + regionID);
            return;
        }

        StartPlaylist(region.playlist);
    }

    // 🔁 START PLAYLIST
    void StartPlaylist(List<AudioClip> playlist)
    {
        if (playlist == null || playlist.Count == 0) return;

        currentPlaylist = playlist;

        if (musicRoutine != null)
            StopCoroutine(musicRoutine);

        musicRoutine = StartCoroutine(PlaylistLoop());
    }

    IEnumerator PlaylistLoop()
    {
        while (true)
        {
            AudioClip nextClip = GetRandomClip();

            yield return StartCoroutine(FadeToNewClip(nextClip));

            yield return new WaitForSeconds(nextClip.length);
        }
    }

    AudioClip GetRandomClip()
    {
        if (currentPlaylist.Count == 1)
            return currentPlaylist[0];

        AudioClip next;
        do
        {
            next = currentPlaylist[Random.Range(0, currentPlaylist.Count)];
        } while (next == lastClip);

        lastClip = next;
        return next;
    }

    IEnumerator FadeToNewClip(AudioClip newClip)
    {
        float t = 0f;
        float startVol = musicSource.volume;

        // Fade out
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, volume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = volume;
    }

    // 🔉 FADE OUT (for scene change)
    public void FadeOutMusic()
    {
        if (musicRoutine != null)
            StopCoroutine(musicRoutine);

        StartCoroutine(FadeOutRoutine());
    }

    IEnumerator FadeOutRoutine()
    {
        float t = 0f;
        float startVol = musicSource.volume;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
    }

    // 🔊 SETTINGS MENU HOOK
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        musicSource.volume = volume;
    }
}