using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject audioMenu;

    [Header("References")]
    [SerializeField] private GraphicsSettingsUI graphicsUI;
    [SerializeField] private AudioSettingsUI audioUI;

    [SerializeField] private bool openedFromPauseMenu;
    [SerializeField] private GameObject pauseMenu;
    // ---------------------------
    // NAVIGATION
    // ---------------------------

    public void OpenGraphics()
    {
        graphicsMenu.SetActive(true);
        audioMenu.SetActive(false);
    }

    public void OpenAudio()
    {
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    // ---------------------------
    // APPLY (BOTH MENUS)
    // ---------------------------
    public void ApplyAndClose()
    {
        graphicsUI.Apply();
        audioUI.Apply();

        Settings.Save();

        CloseMenu();
    }

    void OnEnable()
    {
        Settings.Load();

        graphicsUI.RefreshUI();
       
        audioUI.RefreshUI();
        OpenGraphics();
    }

    // ---------------------------
    // CLOSE MENU (BACK TO GAME)
    // ---------------------------
    public void CloseMenu()
    {
        // Hide settings pages
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(false);

        // Hide settings menu
        gameObject.SetActive(false);

        // Only restore cursor for MAIN MENU context
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}