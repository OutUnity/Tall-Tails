using UnityEngine;
using TMPro;
using System.Collections;

public class NoteUIController : MonoBehaviour
{
    public static NoteUIController Instance;

    public GameObject notePanel;
    public TMP_Text noteTextUI;
    private bool canClose = false;
    private GameObject noteToDestroy;
    public GameObject player;
    private CharacterMovement playerMovement;


    private void Awake()
    {
        Instance = this;
        notePanel.SetActive(false);
        
        playerMovement = player.GetComponent<CharacterMovement>();
       

    }

    public void RegisterNoteToDestroy(GameObject note)
    {
        noteToDestroy = note;
    }

    public void ShowNote(string text)
    {
        notePanel.SetActive(true);
        playerMovement.enabled = false;
        noteTextUI.text = text;
        Debug.Log("Text set to: " + text);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        notePanel.SetActive(true);
        noteTextUI.text = text;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Time.timeScale = 0f;
        StartCoroutine(EnableCloseDelay());
    }
    IEnumerator EnableCloseDelay()
    {
        canClose = false;
        yield return new WaitForSecondsRealtime(0.2f); // small delay
        canClose = true;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMovement.enabled = true;

        // Destroy the note now that the UI is closed
        if (noteToDestroy != null)
        {
            Destroy(noteToDestroy);
            noteToDestroy = null;
        }
    }
}
