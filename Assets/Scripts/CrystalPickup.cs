using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private int regionID;

    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;
    [Range(0f, 2f)] public float volume = 0.3f;

    private void Start()
    {
        RegionTrigger parentRegion = GetComponentInParent<RegionTrigger>();

        if (parentRegion != null)
            regionID = parentRegion.regionID;
        else
            regionID = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CrystalManager.Instance.AddCrystal(regionID);

            // 🔊 Sound
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);

            // ✨ Particles
            if (pickupEffect != null)
            {
                pickupEffect.transform.parent = null;
                pickupEffect.Play();
                Destroy(pickupEffect.gameObject, 2f);
            }

            Destroy(gameObject);
        }
    }
}