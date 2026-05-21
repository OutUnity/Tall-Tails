using UnityEngine;

public class TitleMenuController : MonoBehaviour
{
    [Header("Save Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject loadButton;

    [Header("References")]
    [SerializeField] private GameLoader gameLoader;

    void Start()
    {
        bool hasSave = SaveSystem.HasAnySave();

        if (continueButton != null)
        {
            continueButton.SetActive(hasSave);
        }

        if (loadButton != null)
        {
            loadButton.SetActive(hasSave);
        }
    }

    // =====================================================
    // CONTINUE GAME
    // =====================================================

    public void ContinueGame()
    {
        if (gameLoader != null)
        {
            gameLoader.ContinueGame();
        }
    }

    // =====================================================
    // NEW GAME
    // =====================================================

    public void NewGame()
    {
        if (gameLoader != null)
        {
            gameLoader.NewGame();
        }
    }
}