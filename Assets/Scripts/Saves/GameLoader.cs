using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance;

    [Header("Scene")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    [Header("Loading")]
    [SerializeField] private float minimumLoadTime = 0.5f;

    private bool isLoading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =====================================================
    // MAIN ENTRY (USED BY SAVE SYSTEM)
    // =====================================================
    public void LoadGame()
    {
        if (isLoading)
        {
            return;
        }

        StartCoroutine(LoadSceneRoutine());
    }

    // =====================================================
    // NEW GAME
    // =====================================================
    public void NewGame()
    {
        if (isLoading)
        {
            return;
        }

        SaveSystem.PendingLoad = null;
        StartCoroutine(LoadSceneRoutine());
    }

    // =====================================================
    // CONTINUE GAME
    // =====================================================
    public void ContinueGame()
    {
        if (isLoading)
        {
            return;
        }

        var saves = SaveSystem.GetAllSlots();

        if (saves.Count == 0)
        {
            Debug.LogWarning("No saves found.");
            return;
        }

        SaveSystem.PendingLoad = saves[0].slot;
        StartCoroutine(LoadSceneRoutine());
    }

    // =====================================================
    // LOAD SPECIFIC SLOT
    // =====================================================
    public void LoadSave(int slotIndex)
    {
        if (isLoading)
        {
            return;
        }

        SaveSlot slot = SaveSystem.LoadSlot(slotIndex);

        if (slot == null)
        {
            Debug.LogWarning("Invalid save slot.");
            return;
        }

        SaveSystem.PendingLoad = slot;
        StartCoroutine(LoadSceneRoutine());
    }

    // =====================================================
    // SCENE LOAD
    // =====================================================
    private IEnumerator LoadSceneRoutine()
    {
        isLoading = true;

        if (LoadingUI.Instance != null)
        {
            yield return LoadingUI.Instance.FadeIn();
        }

        yield return new WaitForSeconds(minimumLoadTime);

        yield return SceneManager.LoadSceneAsync(gameplaySceneName);

        isLoading = false;
    }
}