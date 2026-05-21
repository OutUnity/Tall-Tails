using System.Collections;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public static LoadingUI Instance;

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine fadeRoutine;

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
            return;
        }

        Initialize();
    }

    // =====================================================
    // INITIALIZE
    // =====================================================

    private void Initialize()
    {
        if (canvasGroup == null)
        {
            return;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.gameObject.SetActive(false);
    }

    // =====================================================
    // SHOW IMMEDIATELY
    // =====================================================

    public void ShowInstant()
    {
        if (canvasGroup == null)
        {
            return;
        }

        StopFadeRoutine();

        canvasGroup.gameObject.SetActive(true);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // =====================================================
    // HIDE IMMEDIATELY
    // =====================================================

    public void HideInstant()
    {
        if (canvasGroup == null)
        {
            return;
        }

        StopFadeRoutine();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.gameObject.SetActive(false);
    }

    // =====================================================
    // FADE IN
    // =====================================================

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(0f, 1f));
    }

    // =====================================================
    // FADE OUT
    // =====================================================

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(1f, 0f));
    }

    // =====================================================
    // FADE
    // =====================================================

    private IEnumerator Fade(float from, float to)
    {
        if (canvasGroup == null)
        {
            yield break;
        }

        canvasGroup.gameObject.SetActive(true);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(from, to, timer / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = to;

        if (to <= 0f)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            canvasGroup.gameObject.SetActive(false);
        }
    }

    // =====================================================
    // PLAY FADE IN
    // =====================================================

    public void PlayFadeIn()
    {
        StopFadeRoutine();

        fadeRoutine =
            StartCoroutine(FadeIn());
    }

    // =====================================================
    // PLAY FADE OUT
    // =====================================================

    public void PlayFadeOut()
    {
        StopFadeRoutine();

        fadeRoutine =
            StartCoroutine(FadeOut());
    }

    // =====================================================
    // STOP ROUTINE
    // =====================================================

    private void StopFadeRoutine()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);

            fadeRoutine = null;
        }
    }
}