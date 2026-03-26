using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange : MonoBehaviour
{
    public string sceneName;
    public float fadeDuration = 1.5f;
    public int startingRegionID = 1;

    public void ChangeScene()
    {
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.FadeOutMusic();

        yield return new WaitForSeconds(fadeDuration);

        SceneManager.LoadScene(sceneName);

        yield return null; // wait 1 frame

        // ✅ STEP 4 GOES HERE
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetRegion(1);

        if (CrystalManager.Instance != null)
            CrystalManager.Instance.SetCurrentRegion(1);
    }
}
