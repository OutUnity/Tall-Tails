using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private int regionID;
    private void Start()
    {
        // Auto-detect region by parent
        RegionTrigger parentRegion = GetComponentInParent<RegionTrigger>();
        if (parentRegion != null)
        {
            regionID = parentRegion.regionID;
        }
        else
        {
            Debug.LogWarning("CrystalPickup could not find parent RegionTrigger!");
            regionID = 1; // fallback
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CrystalManager.Instance.AddCrystal(regionID);
            Destroy(gameObject);
        }
    }
}
