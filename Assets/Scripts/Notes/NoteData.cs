using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNoteList", menuName = "Collectible System/Note List")]
public class NoteData : ScriptableObject
{
    [TextArea(3, 10)] // Makes it easier to type long notes in the Inspector
    public List<string> allPossibleTexts;
}
