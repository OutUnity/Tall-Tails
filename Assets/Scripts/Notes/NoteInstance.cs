using UnityEngine;

public class NoteInstance : MonoBehaviour
{
    // The spawner will fill this variable with a unique string
    public string noteText;
    public float hoverHeight = 0.5f;    // How high it moves up/down
    public float hoverSpeed = 2f;

    public Transform playerTransform; // assign your player or camera
    public bool facePlayer = true;

    public AudioClip collectSound;
    public ParticleSystem collectParticles;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        if (playerTransform == null)
        {
            // Try to find the player by tag if not assigned
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
    }
    private void Update()
    {
        // Hover
        Vector3 pos = startPos;
        pos.y += Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = pos;

        // Optional spin
        transform.Rotate(Vector3.up, 50f * Time.deltaTime, Space.World);

        // Face player
        if (facePlayer && playerTransform != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0; // horizontal only
            transform.rotation = Quaternion.LookRotation(direction);
            transform.Rotate(90f, 0f, 0f, Space.Self); // make plane stand upright
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // Debug.Log("Trigger worked. Text is: " + noteText);
            
            NoteUIController.Instance.ShowNote(noteText);
            NoteUIController.Instance.RegisterNoteToDestroy(this.gameObject);
            // Play collection effects
            PlayEffects();

            // Disable visuals immediately so player doesn't see duplicate
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
    private void PlayEffects()
    {
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        if (collectParticles != null)
        {
            ParticleSystem particles = Instantiate(collectParticles, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }
}
