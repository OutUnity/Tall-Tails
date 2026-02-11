using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    public static CharacterHealth Instance; // Singleton instance for easy access

    public int maxHealth = 100; // Maximum health of the character
    public int currentHealth; // Current health of the character

    public Image healthBar; // Reference to the UI Image component representing the health bar

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool Regenerate = true; // Flag to enable or disable health regeneration
    public float regen = 0.1f; // Amount of health regenerated per second
    private float timeleft = 0.0f; // Time left for the current regeneration interval
    public float regenUpdateInterval = 1f; // Time interval for health regeneration updates


    void Awake()
    {
        Instance = this; // Set the singleton instance to this object
    }
    void Start()
    {
        timeleft = regenUpdateInterval; // Initialize the time left for health regeneration
        currentHealth = maxHealth; // Initialize current health to maximum health at the start
        UpdateHealthBar(); // Update the health bar UI to reflect the initial health
       

    }
    void Update()
    {
        if (Regenerate) 
        {
            Regen();
        }  
    }

    private void Regen() 
    {
        timeleft -= Time.deltaTime; // Decrease the time left by the time elapsed since the last frame
        if (timeleft <= 0) 
        {
            heal((int)regen); // Heal the character by the specified regeneration amount
            timeleft = regenUpdateInterval;
          
        }
    }
    public void heal(int amount) 
    {
        currentHealth += amount; // Increase current health by a fixed amount (e.g., 20)
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp current health between 0 and maxHealth
        UpdateHealthBar(); // Update the health bar UI to reflect the new health value
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage taken
        if (currentHealth < 0)
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <=0) 
        {
            // Handle character death (e.g., play death animation, disable character controls, etc.)
            Debug.Log("Character has died!");
        }
        UpdateHealthBar(); // Update the health bar UI to reflect the new health value
    }
    // Update is called once per frame
    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        healthBar.rectTransform.localPosition = new Vector3(healthBar.rectTransform.rect.width * healthBar.fillAmount - healthBar.rectTransform.rect.width, 0, 0);
    }

}
