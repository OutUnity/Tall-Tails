using UnityEngine;

public class PlayTimeManager : MonoBehaviour
{
    public static PlayTimeManager Instance;

    private float totalPlayTime;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        totalPlayTime += Time.deltaTime;
    }

    public float GetPlayTime()
    {
        return totalPlayTime;
    }

    public void SetPlayTime(float time)
    {
        totalPlayTime = time;
    }
}