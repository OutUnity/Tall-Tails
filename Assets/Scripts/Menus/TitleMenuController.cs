using UnityEngine;

public class TitleMenuController : MonoBehaviour
{
    [Header("Save Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject loadButton;

    void Start()
    {
        bool hasSave =
            PlayerPrefs.HasKey("SavedScene");

        // Only show if save exists
        continueButton.SetActive(hasSave);
        loadButton.SetActive(hasSave);
    }
}