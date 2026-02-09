using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string[] sceneNames; // Array to hold the names of the scenes
    public int nextSceneIndex = 0; // Index to track the current scene

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneNames[nextSceneIndex]);
    }
}
