using UnityEngine;

public class TitleMenuController : MonoBehaviour
{
    [Header("Save Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject loadButton;

    void Start()
    {
        bool hasSave = SaveSystem.HasAnySave();

        continueButton.SetActive(hasSave);
        loadButton.SetActive(hasSave);
    }
}