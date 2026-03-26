using UnityEngine;

public class PlayerRegionDetector : MonoBehaviour
{
    private int currentRegion = -1;

    void Update()
    {
        int region = RegionMapManager.Instance.GetRegionFromPosition(transform.position);

        if (region != currentRegion && region != -1)
        {
            currentRegion = region;

            CrystalManager.Instance.SetCurrentRegion(region);

            if (MusicManager.Instance != null)
                MusicManager.Instance.SetRegion(region);

            Debug.Log("Entered Region: " + region);
        }
    }
}