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
        if (fadeGroup != null)
            fadeGroup.alpha = 0f;  // start transparent
    }
    void Update()
    {
        HandleKeyboardInput();
       // HandleControllerInput();
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState != MenuState.Closed)
                CloseMenu();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (currentState == MenuState.Closed)
                OpenMap();
            else
                SwitchToMap();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (currentState == MenuState.Closed)
                OpenNotes();
            else
                SwitchToNotes();
        }
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
        if (isTransitioning || currentState != MenuState.Closed) return;

        OpenMenu();

        mapPanel.SetActive(true);
        notesPanel.SetActive(false);

        StartCoroutine(Fade(1f));
        currentState = MenuState.Map;
    }

    public void OpenNotes()
    {
        if (isTransitioning || currentState != MenuState.Closed) return;

        OpenMenu();

        notesPanel.SetActive(true);
        mapPanel.SetActive(false);

        StartCoroutine(Fade(1f));
        currentState = MenuState.Notes;
    }

    void OpenMenu()
    {
        mainMenuPanel.SetActive(true);

        if (hudPanel != null)
            hudPanel.SetActive(false);

        Time.timeScale = 0f;

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        fadeGroup.alpha = 0f;
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
        if (isTransitioning || currentState == MenuState.Map) return;
        StartCoroutine(FadeSwitch(mapPanel, notesPanel, MenuState.Map));
    }

    void SwitchToNotes()
    {
        if (isTransitioning || currentState == MenuState.Notes) return;
        StartCoroutine(FadeSwitch(notesPanel, mapPanel, MenuState.Notes));
    }

    IEnumerator FadeSwitch(GameObject show, GameObject hide, MenuState newState)
    {
        isTransitioning = true;

        // Fade out
        yield return Fade(0f);

        hide.SetActive(false);
        show.SetActive(true);

        // Fade in
        yield return Fade(1f);

        currentState = newState;
        isTransitioning = false;
    }

    IEnumerator Fade(float target)
    {
        float duration = 0.15f;
        float start = fadeGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            fadeGroup.alpha = Mathf.Lerp(start, target, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeGroup.alpha = target;
    }
}