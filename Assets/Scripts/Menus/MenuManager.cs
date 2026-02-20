using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public enum MenuState
    {
        Closed,
        Map,
        Notes
    }

    public MenuState currentState = MenuState.Closed;

    [Header("UI References")]
    public GameObject mainMenuPanel;
    public GameObject mapPanel;
    public GameObject notesPanel;
    public CanvasGroup fadeGroup;
    public GameObject hudPanel;

    [Header("Player")]
    public MonoBehaviour playerMovement; // drag CharacterMovement here

    private bool isTransitioning = false;

    void Start()
    {
        mainMenuPanel.SetActive(false);
        mapPanel.SetActive(false);
        notesPanel.SetActive(false);
        fadeGroup.alpha = 0f;

    }
    void Update()
    {
        HandleKeyboardInput();
       // HandleControllerInput();
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.M)) OpenMap();
        if (Input.GetKeyDown(KeyCode.N)) OpenNotes();
    }
    /*
    void HandleControllerInput()
    {
        if (currentState == MenuState.Closed) return;

        if (Input.GetButtonDown("RB"))
            SwitchToNotes();

        if (Input.GetButtonDown("LB"))
            SwitchToMap();
    }
    */
    public void OpenMap()
    {
        if (isTransitioning) return;

        OpenMenu();
        SwitchToMap();
    }

    public void OpenNotes()
    {
        if (isTransitioning) return;
        if (hudPanel != null)
        {
            hudPanel.SetActive(false);
        }
        OpenMenu();
        SwitchToNotes();
    }

    void OpenMenu()
    {
        if (hudPanel != null) { 
            hudPanel.SetActive(false);
        }
        mainMenuPanel.SetActive(true);
        Time.timeScale = 0f;

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentState = MenuState.Map;
    }

    public void CloseMenu()
    {
        mainMenuPanel.SetActive(false);
        if (hudPanel != null) { 
            hudPanel.SetActive(true); 
        }
        Time.timeScale = 1f;

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentState = MenuState.Closed;
    }

    void SwitchToMap()
    {
        if (currentState == MenuState.Map) return;

        StartCoroutine(FadeSwitch(mapPanel, notesPanel));
        currentState = MenuState.Map;
    }

    void SwitchToNotes()
    {
        if (currentState == MenuState.Notes) return;

        StartCoroutine(FadeSwitch(notesPanel, mapPanel));
        currentState = MenuState.Notes;
    }

    IEnumerator FadeSwitch(GameObject show, GameObject hide)
    {
        isTransitioning = true;

        yield return Fade(0f);

        hide.SetActive(false);
        show.SetActive(true);

        yield return Fade(1f);

        isTransitioning = false;
    }

    IEnumerator Fade(float target)
    {
        float duration = 0.15f;
        float start = fadeGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            fadeGroup.alpha = Mathf.Lerp(start, target, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeGroup.alpha = target;
    }
}