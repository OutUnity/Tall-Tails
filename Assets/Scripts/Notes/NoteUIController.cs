using UnityEngine;
using TMPro;

public class NoteUIController : MonoBehaviour
{
    public static NoteUIController Instance;

    public GameObject notePanel;
    public TMP_Text noteTextUI;

    private void Awake()
    {
        Instance = this;
        notePanel.SetActive(false);
    }

    public void ShowNote(string text)
    {
        notePanel.SetActive(true);
        noteTextUI.text = text;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}
