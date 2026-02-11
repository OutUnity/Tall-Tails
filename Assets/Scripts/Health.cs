using UnityEngine;

public class Health : MonoBehaviour
{
    public int healAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        CharacterHealth playerHealth = other.GetComponent<CharacterHealth>();

        if (playerHealth != null)
        {
            playerHealth.heal(healAmount);
            Destroy(gameObject);
        }
    }
}
