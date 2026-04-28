using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Rotation")]
    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float cameraSmooth = 0.1f;

    [Header("Camera Limits")]
    [SerializeField] private float maxLookUp = 60f;
    [SerializeField] private float maxLookDown = -20f;

    [Header("Camera Feel")]
    [SerializeField] private float cameraSoftness = 5f;

    [Header("References")]
    [SerializeField] private Transform visual;
    [SerializeField] private Transform cameraHolder;

    [Header("Dash")]
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.2f;

    private bool isDashing;
    private bool canDash = true;
    private float dashTimeLeft;
    private float dashCooldownTimer;

    private Rigidbody rb;
    private Animator animator;

    private float xRotation = 0f;
    private float targetXRotation = 0f;

    // Smooth turn system
    private float turnSmoothVelocity;
    private float currentTurnValue;

    private float cameraRotVelocity;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = visual.GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseLook();
        HandleJump();
        HandleDashInput();
        HandleDashTimers();

        HandleAnimations();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        HandleDashMovement();
    }

    // ---------------- MOVEMENT ----------------

    void HandleMovement()
    {
        if (isDashing)
        {
            return;
        }

        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = visual.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 moveDirection = forward * vertical;

        float speedMultiplier = 1f;

        if (vertical > 0f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speedMultiplier = runMultiplier;
            }
        }

        Vector3 velocity = moveDirection * moveSpeed * speedMultiplier;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    // ---------------- ROTATION (VISUAL ONLY) ----------------

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");

        float rawTurn = mouseX;

        if (rawTurn < -1f)
        {
            rawTurn = -1f;
        }
        else if (rawTurn > 1f)
        {
            rawTurn = 1f;
        }

        currentTurnValue = Mathf.SmoothDamp(
            currentTurnValue,
            rawTurn,
            ref turnSmoothVelocity,
            turnSmoothTime
        );

        float rotationAmount = currentTurnValue * turnSpeed * Time.fixedDeltaTime;

        visual.Rotate(0f, rotationAmount, 0f);
    }

    // ---------------- CAMERA (SOFT PITCH SYSTEM) ----------------

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal player rotation
        transform.Rotate(Vector3.up * mouseX);

        // Vertical camera input
        targetXRotation -= mouseY;

        // SOFT LIMIT SYSTEM (elastic resistance)
        if (targetXRotation > maxLookUp)
        {
            float excess = targetXRotation - maxLookUp;
            targetXRotation -= excess * Time.deltaTime * cameraSoftness;
        }
        else if (targetXRotation < maxLookDown)
        {
            float excess = maxLookDown - targetXRotation;
            targetXRotation += excess * Time.deltaTime * cameraSoftness;
        }

        // Smooth camera motion
        xRotation = Mathf.SmoothDamp(
            xRotation,
            targetXRotation,
            ref cameraRotVelocity,
            cameraSmooth
        );

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // ---------------- JUMP ----------------

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;

                animator.SetTrigger("Jump");
            }
        }
    }

    // ---------------- DASH ----------------

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash)
            {
                StartDash();
            }
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
    }

    void HandleDashMovement()
    {
        if (!isDashing)
        {
            return;
        }

        Vector3 forward = visual.forward;
        forward.y = 0f;
        forward.Normalize();

        rb.MovePosition(transform.position + forward * dashForce * Time.fixedDeltaTime);
    }

    void HandleDashTimers()
    {
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;

            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;

            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
            }
        }
    }

    // ---------------- ANIMATION ----------------

    void HandleAnimations()
    {
        float vertical = Input.GetAxis("Vertical");

        float speed = 0f;

        if (Mathf.Abs(vertical) < 0.1f)
        {
            speed = 0f;
        }
        else
        {
            if (vertical > 0f)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = 1f;
                }
                else
                {
                    speed = 0.5f;
                }
            }
            else
            {
                speed = -0.5f;
            }
        }

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", currentTurnValue, 0.1f, Time.deltaTime);
        animator.SetBool("Grounded", isGrounded);
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
}