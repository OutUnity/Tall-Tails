using UnityEngine;
using UnityEngine.SceneManagement;

public class creditsScript : MonoBehaviour
{
    public float scrollSpeed = 20f; // Speed at which the credits will scroll
    private RectTransform rectTransform; // Reference to the RectTransform component
    public float scrollLimit = 1000f; // The y position at which the credits will stop scrolling and change scene


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component attached to this GameObject
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;// Move the credits upwards based on the scroll speed and time
        if(rectTransform.anchoredPosition.y >= scrollLimit) // Check if the credits have scrolled past a certain point (you can adjust this value as needed)
        {
            SceneManager.LoadScene("MainMenu"); // Load the MainMenu scene when the credits have finished scrolling
        }
    }
}
