using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject pauseCanvas;

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    [Header("Settings UI References")]
    [SerializeField] private GraphicsSettingsUI graphicsUI;
    [SerializeField] private AudioSettingsUI audioUI;

    [Header("Sub Menus")]
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject volumeMenu;


    [Header("State")]
    private bool isPaused = false;


    void Start()
    {
        // IMPORTANT: ensure it starts OFF
        pauseCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                OpenPause();
            else
                ClosePause();
        }
    }

    // -------------------------
    // OPEN PAUSE
    // -------------------------
    public void OpenPause()
    {
        isPaused = true;

        pauseCanvas.SetActive(true);

        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // -------------------------
    // CLOSE PAUSE
    // -------------------------
    public void ClosePause()
    {
        isPaused = false;

        pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // -------------------------
    // OPEN SETTINGS
    // -------------------------
    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        graphicsMenu.SetActive(true);
        volumeMenu.SetActive(false);
    }

    // -------------------------
    // GRAPHICS -> VOLUME
    // -------------------------
    public void OpenVolume()
    {
        graphicsMenu.SetActive(false);
        volumeMenu.SetActive(true);
    }

    // -------------------------
    // VOLUME -> GRAPHICS
    // -------------------------
    public void BackToGraphics()
    {
        volumeMenu.SetActive(false);
        graphicsMenu.SetActive(true);
    }

    // -------------------------
    // BACK TO PAUSE MENU
    // -------------------------
    public void BackToPauseMenu()
    {

        // Disable entire settings system cleanly
        settingsMenu.SetActive(false);

        graphicsMenu.SetActive(false);
        volumeMenu.SetActive(false);

        // Return to pause menu
        pauseMenu.SetActive(true);
    }
    public void ApplyGraphicsAndReturn()
    {
        graphicsUI.Apply();
        Settings.Save();

        BackToPauseMenu();
    }
    public void ApplyAudioAndReturn()
    {
        audioUI.Apply();
        Settings.Save();

        BackToPauseMenu();
    }
    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }
}