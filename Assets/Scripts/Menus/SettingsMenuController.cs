using UnityEngine;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject audioMenu;

    [Header("References")]
    [SerializeField] private GraphicsSettingsUI graphicsUI;
    [SerializeField] private AudioSettingsUI audioUI;

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
    }

    // ---------------------------
    // CLOSE MENU (BACK TO GAME)
    // ---------------------------
    public void CloseMenu()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1f; // resume game if paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}