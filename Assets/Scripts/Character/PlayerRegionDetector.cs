using UnityEngine;
using System.Collections;

public class PlayerRegionDetector : MonoBehaviour
{
    private int currentRegion = -1;

    void Update()
    {

      

            int region = RegionMapManager.Instance.GetRegionFromPosition(transform.position);

        if (region == -1) return; // ignore invalid pixels

        if (region != currentRegion)
        {
            currentRegion = region;

            CrystalManager.Instance.SetCurrentRegion(region);

            if (MusicManager.Instance != null)
                MusicManager.Instance.SetRegion(region);

            //Debug.Log("Entered Region: " + region);

            // 🎯 Show UI overlay
            RegionUI.Instance.ShowRegion(region);
        }
        //Debug.Log("Current Region: " + currentRegion);
    }
}
    