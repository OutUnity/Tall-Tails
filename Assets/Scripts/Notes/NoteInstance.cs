using UnityEngine;

public class NoteInstance : MonoBehaviour
{
    // The spawner will fill this variable with a unique string
    public string noteText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NoteUIController.Instance.ShowNote(noteText);
            Destroy(gameObject);
        }
    }
}
