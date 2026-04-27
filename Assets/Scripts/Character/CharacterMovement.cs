using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.2f;
    [SerializeField] private float runMultiplier = 1.5f;

    private bool isDashing;
    private bool canDash = true;
    private float dashTimeLeft;
    private float dashCooldownTimer;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraHolder;

    [Header("Camera Bob")]
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float bobAmplitude = 0.05f;

    [Header("Camera Shake")]
    [SerializeField] private float shakeIntensity = 0.15f;
    [SerializeField] private float shakeDecay = 6f;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded;

    private Animator animator;
    private float idleTimer;
    [SerializeField] private float idleThreshold = 2f;

    private Rigidbody rb;

    private float xRotation = 0f;
    private float targetXRotation = 0f;

    private Vector3 baseCameraPos;
    private float bobTimer;

    private float shakeStrength;
    private float shakeTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        xRotation = cameraHolder.localEulerAngles.x;
        if (xRotation > 180f)
            xRotation -= 360f;

        targetXRotation = xRotation;

        baseCameraPos = cameraHolder.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseLook();
        HandleJump();
        HandleDashInput();
        HandleDashTimers();

        HandleCameraBob();
        HandleCameraShake();
        HandleAnimations();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleDashMovement();
    }

    void HandleMovement()
    {
        if (isDashing) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraHolder.forward;
        Vector3 right = cameraHolder.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? runMultiplier : 1f;

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized * speedMultiplier;

        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
    void HandleAnimations()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);
        float speed = input.magnitude;

        // Send speed to animator
        animator.SetFloat("Speed", speed * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));

        // Ground state
        animator.SetBool("Grounded", isGrounded);

        // Idle timer logic
        if (speed > 0.1f)
        {
            idleTimer = 0f;
        }
        else
        {
            idleTimer += Time.deltaTime;
        }

        // Optional: trigger AFK idle (if you add animation state later)
        if (idleTimer > idleThreshold)
        {
            // Example hook (only if you add AFK idle animation state)
            // animator.SetTrigger("IdleAFK");
        }
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        targetXRotation -= mouseY;
        targetXRotation = Mathf.Clamp(targetXRotation, -40f, 60f);

        xRotation = Mathf.Lerp(xRotation, targetXRotation, 0.1f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            animator.SetTrigger("Jump");
        }
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;

        shakeStrength = shakeIntensity;
    }

    void HandleDashMovement()
    {
        if (!isDashing) return;

        Vector3 forward = cameraHolder.forward;
        forward.y = 0f;
        forward.Normalize();

        rb.MovePosition(transform.position + forward * dashForce * Time.fixedDeltaTime);
    }

    void HandleDashTimers()
    {
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0)
            {
                canDash = true;
            }
        }
    }

    void HandleCameraBob()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (isMoving && isGrounded && !isDashing)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;

            baseCameraPos.y = cameraHolder.localPosition.y;
            baseCameraPos.y = 2f; // ensure stable baseline
            baseCameraPos = cameraHolder.parent.InverseTransformPoint(cameraHolder.parent.TransformPoint(baseCameraPos));

            cameraHolder.localPosition = baseCameraPos + new Vector3(0f, bobOffset, 0f);
        }
        else
        {
            bobTimer = 0f;
            cameraHolder.localPosition = Vector3.Lerp(
                cameraHolder.localPosition,
                baseCameraPos,
                Time.deltaTime * 5f
            );
        }
    }

    void HandleCameraShake()
    {
        Vector3 shakeOffset = Vector3.zero;

        if (shakeStrength > 0)
        {
            shakeTimer += Time.deltaTime * 25f;

            float rumble = Mathf.Sin(shakeTimer) * shakeStrength;

            shakeOffset = new Vector3(rumble, 0f, 0f);

            shakeStrength -= Time.deltaTime * shakeDecay;
        }
        else
        {
            shakeTimer = 0f;
        }

        // final camera composition (prevents drift)
        cameraHolder.localPosition = baseCameraPos + shakeOffset;
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
}