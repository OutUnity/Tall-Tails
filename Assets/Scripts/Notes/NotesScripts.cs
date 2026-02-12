using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesScripts : MonoBehaviour
{
    public NoteData noteData; // Assign your ScriptableObject here
    public GameObject notePrefab;
    public List<Transform> spawnPoints;

    private List<string> availableTexts;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableTexts = new List<string>(noteData.allPossibleTexts);
        SpawnNotes(spawnPoints.Count);
    }
    void SpawnNotes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Pick random indices
            int textIndex = Random.Range(0, availableTexts.Count);
            int spotIndex = Random.Range(0, spawnPoints.Count);

            // Spawn the note at the selected spot
            GameObject newNote = Instantiate(notePrefab, spawnPoints[spotIndex].position, spawnPoints[spotIndex].rotation);

            // Assign the unique text to the NoteInstance script on the prefab
            NoteInstance script = newNote.GetComponent<NoteInstance>();
            if (script != null)
            {
                script.noteText = availableTexts[textIndex];
            }
            else
            {
                Debug.LogWarning("NoteSpawner: The Note Prefab is missing the NoteInstance script!");
            }

            // 4. Remove used data so they don't repeat
            availableTexts.RemoveAt(textIndex);
            spawnPoints.RemoveAt(spotIndex);
        }
    }
}
