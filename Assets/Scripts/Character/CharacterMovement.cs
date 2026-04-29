using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Visual Rotation")]
    [SerializeField] private float visualTurnSmooth = 10f;

    [Header("Camera")]
    [SerializeField] private float cameraSmooth = 0.1f;

    [Header("Camera Limits")]
    [SerializeField] private float maxLookUp = 60f;
    [SerializeField] private float maxLookDown = -20f;

    [Header("Camera Feel")]
    [SerializeField] private float cameraSoftness = 5f;

    [Header("Hybrid Turning")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float edgeTurnStrength = 120f;
    [SerializeField] private float edgeExponent = 1.5f;

    [Header("Recenter")]
    [SerializeField] private float recenterSpeed = 2f;
    [SerializeField] private float recenterDeadZone = 0.1f;

    [Header("Camera Follow Lag")]
    [SerializeField] private float cameraFollowSmooth = 5f;

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
    private float cameraRotVelocity;

    private float targetYRotation;
    private float currentYRotation;

    private bool isGrounded;

    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = visual.GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        targetYRotation = transform.eulerAngles.y;
        currentYRotation = targetYRotation;
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
        HandleVisualRotation();
        HandleDashMovement();
        HandleCameraFollowLag();
    }

    // ---------------- MOVEMENT ----------------

    void HandleMovement()
    {
        if (isDashing)
        {
            return;
        }

        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
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

    // ---------------- VISUAL ROTATION ----------------

    void HandleVisualRotation()
    {
        Vector3 currentForward = visual.forward;
        Vector3 targetForward = transform.forward;

        currentForward.y = 0f;
        targetForward.y = 0f;

        currentForward.Normalize();
        targetForward.Normalize();

        if (currentForward != targetForward)
        {
            Vector3 newDirection = Vector3.Slerp(
                currentForward,
                targetForward,
                visualTurnSmooth * Time.deltaTime
            );

            visual.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    // ---------------- CAMERA + HYBRID TURN ----------------

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Vector2 mousePos = Input.mousePosition;

        float centerX = Screen.width * 0.5f;
        float centerY = Screen.height * 0.5f;

        float offsetX = (mousePos.x - centerX) / centerX;
        float offsetY = (mousePos.y - centerY) / centerY;

        float edgeX = Mathf.Sign(offsetX) * Mathf.Pow(Mathf.Abs(offsetX), edgeExponent);
        float edgeY = Mathf.Sign(offsetY) * Mathf.Pow(Mathf.Abs(offsetY), edgeExponent);

        float finalTurn = mouseX + (edgeX * edgeTurnStrength * Time.deltaTime);
        float finalLookY = mouseY + (edgeY * edgeTurnStrength * Time.deltaTime);

        turnInput = finalTurn;

        // store target rotation instead of applying directly
        targetYRotation += finalTurn;

        targetXRotation -= finalLookY;

        // --- RECENTER ---
        if (Mathf.Abs(offsetX) < recenterDeadZone)
        {
            turnInput = Mathf.Lerp(turnInput, 0f, Time.deltaTime * recenterSpeed);
        }

        if (Mathf.Abs(offsetY) < recenterDeadZone)
        {
            float centerTarget = Mathf.Clamp(targetXRotation, maxLookDown, maxLookUp);
            targetXRotation = Mathf.Lerp(targetXRotation, centerTarget, Time.deltaTime * recenterSpeed);
        }

        // --- SOFT LIMITS ---
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

        xRotation = Mathf.SmoothDamp(
            xRotation,
            targetXRotation,
            ref cameraRotVelocity,
            cameraSmooth
        );

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // ---------------- CAMERA FOLLOW LAG ----------------

    void HandleCameraFollowLag()
    {
        currentYRotation = Mathf.Lerp(
            currentYRotation,
            targetYRotation,
            Time.fixedDeltaTime * cameraFollowSmooth
        );

        transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
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

        Vector3 forward = transform.forward;
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

        float smoothedTurn = Mathf.Lerp(
            animator.GetFloat("Turn"),
            turnInput,
            Time.deltaTime * 10f
        );

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", smoothedTurn, 0.1f, Time.deltaTime);
        animator.SetBool("Grounded", isGrounded);
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
}