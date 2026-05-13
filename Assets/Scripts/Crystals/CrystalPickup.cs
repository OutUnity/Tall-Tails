using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    private int regionID;

    [Header("Effects")]
    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;

    [Header("Audio Settings")]
    [Range(0f, 1f)] public float volume = 0.3f;       // Master volume for this pickup
    [Range(0f, 1f)] public float spatialBlend = 0.3f; // 0 = 2D, 1 = fully 3D
    [Range(0.8f, 1.2f)] public float randomPitchMin = 0.95f;
    [Range(0.8f, 1.2f)] public float randomPitchMax = 1.05f;

    private void Start()
    {
        RegionTrigger parentRegion = GetComponentInParent<RegionTrigger>();
        if (parentRegion != null)
            regionID = parentRegion.regionID;
        else
            regionID = 1; // fallback
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Add crystal to manager
        CrystalManager.Instance.AddCrystal(regionID);

        // --- Sound ---
        if (pickupSound != null)
        {
            // Create temporary AudioSource for 3D playback
            GameObject tempAudio = new GameObject("TempPickupAudio");
            tempAudio.transform.position = transform.position;
            AudioSource aSource = tempAudio.AddComponent<AudioSource>();

            aSource.clip = pickupSound;
            aSource.volume = volume;
            aSource.spatialBlend = spatialBlend;
            aSource.pitch = Random.Range(randomPitchMin, randomPitchMax);
            aSource.Play();

            Destroy(tempAudio, pickupSound.length + 0.1f);
        }

        // --- Particles ---
        if (pickupEffect != null)
        {
            pickupEffect.transform.parent = null; // detach so it keeps playing
            pickupEffect.Play();
            Destroy(pickupEffect.gameObject, pickupEffect.main.duration + pickupEffect.main.startLifetime.constantMax);
        }

        // Destroy the crystal
        Destroy(gameObject);
    }
}