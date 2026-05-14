using UnityEngine;

public class RegionTrigger : MonoBehaviour
{
    public int regionID;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CrystalManager.Instance.SetCurrentRegion(regionID);

        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetRegion(regionID);
        }
    }
}
