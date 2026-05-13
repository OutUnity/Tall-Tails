using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static void SaveGame()
    {
        PlayerPrefs.SetString("SavedScene", SceneManager.GetActiveScene().name);

        // Example player data (you will expand this later)
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Vector3 pos = player.transform.position;

            PlayerPrefs.SetFloat("PlayerX", pos.x);
            PlayerPrefs.SetFloat("PlayerY", pos.y);
            PlayerPrefs.SetFloat("PlayerZ", pos.z);
        }

        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        string scene = PlayerPrefs.GetString("SavedScene", "");

        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
    }
}