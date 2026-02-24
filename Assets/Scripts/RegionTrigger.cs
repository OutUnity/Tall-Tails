using UnityEngine;

public class RegionTrigger : MonoBehaviour
{
    [Tooltip("Region ID assigned in CrystalManager")]
    public int regionID = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CrystalManager.Instance.SetCurrentRegion(regionID);
        }
    }
}
