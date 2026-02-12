using UnityEngine;

public class Damage : MonoBehaviour
{
  
    public int dmgAmount = 25;

    private void OnTriggerEnter(Collider other)
    {
        CharacterHealth playerHealth = other.GetComponent<CharacterHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(dmgAmount);
            Destroy(gameObject);
        }
    }

}
