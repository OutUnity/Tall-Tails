using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f; // Speed at which the character moves
    [SerializeField] public float jumpForce = 10f; // Force applied when the character jumps
    [SerializeField] private float xRotation = 0f; // Limit for the character's rotation on the x-axis
    [SerializeField] private bool isGrounded; // Flag to check if the character is on the ground

    //mouse look variables
    [SerializeField] private float mouseSensitivity = 100f; // Sensitivity for mouse movement

    public Transform cameraHolder;
    private Rigidbody rb; // Reference to the Rigidbody component for physics-based movement

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to this GameObject
        rb.freezeRotation = true; // Prevent the Rigidbody from rotating due to physics interactions
    }
    private void Update()
    {
        mouseLook();
        jump(); // Call the jump method to check for jump input and apply jump force if necessary
    }
    private void FixedUpdate()
    {
        // Get input for horizontal and vertical movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on input and camera orientation
        Vector3 moveDirection = (cameraHolder.forward * vertical + cameraHolder.right * horizontal).normalized;
        // Move the character in the calculated direction
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Check for jump input and if the character is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an upward force to make the character jump
            isGrounded = false; // Set grounded flag to false until the character lands again
        }
    }
    void mouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Get horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Get vertical mouse movement
        xRotation -= mouseY; // Adjust the xRotation based on vertical mouse movement
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the xRotation to prevent excessive rotation
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate the camera holder based on xRotation
        transform.Rotate(Vector3.up * mouseX); // Rotate the character horizontally based on horizontal mouse movement
    }
    void jump() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        isGrounded = true; // Set grounded flag to true when the character is in contact with a surface (e.g., the ground)
    }

}
