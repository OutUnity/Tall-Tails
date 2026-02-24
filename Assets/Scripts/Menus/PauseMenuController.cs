using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel;        // assign the panel in inspector
    public MenuManager menuManager;
    public MonoBehaviour playerMovement;     // your CharacterMovement script
    public GameObject hudPanel;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuManager.currentState == MenuManager.MenuState.Closed)
            {
                if (!isPaused)
                    OpenPauseMenu();
                else
                    ClosePauseMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (hudPanel != null)
            hudPanel.SetActive(false); // hide HUD

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }

    public void ClosePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (playerMovement != null)
            playerMovement.enabled = true;
        if (hudPanel != null)
            hudPanel.SetActive(true); // restore HUD

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // ensure time is running
        // Load your main menu scene here, e.g.:
        SceneManager.LoadScene("MainMenu");
    }
}