using UnityEngine;

public class RegionTrigger : MonoBehaviour
{
    public int regionID;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Tell CrystalManager (you already do this probably)
        CrystalManager.Instance.SetCurrentRegion(regionID);

        // 🎵 Tell MusicManager
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetRegion(regionID);
        }
    }
}
