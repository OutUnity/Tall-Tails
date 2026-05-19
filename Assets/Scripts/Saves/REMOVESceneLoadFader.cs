using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadFader : MonoBehaviour
{
    public static SceneLoadFader Instance;

    [SerializeField] private float fadeDelay = 0.1f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(fadeDelay);

        if (LoadingUI.Instance != null)
        {
            LoadingUI.Instance.Hide();
        }
    }

    public void FadeOut()
    {
        if (LoadingUI.Instance != null)
        {
            LoadingUI.Instance.Show();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}