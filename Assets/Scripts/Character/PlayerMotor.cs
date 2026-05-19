using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1.2f;

    private Rigidbody rb;

    private bool isDashing;
    private bool canDash = true;
    private float dashTimeLeft;
    private float dashCooldownTimer;

    private bool isGrounded;
    private bool isLocked; // NEW (used during loading)

    public Transform cameraTransform;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    public bool IsDashing => isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isLocked)
        {
            VerticalInput = 0f;
            HorizontalInput = 0f;
            return;
        }

        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");

        HandleDashTimers();
    }

    // =========================================================
    // PUBLIC: CALLED AFTER LOAD
    // =========================================================
    public void ResetAfterLoad()
    {
        isDashing = false;
        canDash = true;
        dashTimeLeft = 0f;
        dashCooldownTimer = 0f;
        isLocked = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void SetLocked(bool value)
    {
        isLocked = value;

        if (value)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // =========================================================
    // MOVEMENT
    // =========================================================
    public void HandleMovement(PlayerState state)
    {
        if (isLocked)
        {
            return;
        }

        if (state != PlayerState.Grounded || isDashing)
        {
            return;
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection =
            camForward * VerticalInput +
            camRight * HorizontalInput;

        moveDirection.Normalize();

        float speed = moveSpeed;

        if (VerticalInput > 0f && Input.GetKey(KeyCode.LeftShift))
        {
            speed *= runMultiplier;
        }

        Vector3 velocity =
            moveDirection * speed;

        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    // =========================================================
    // JUMP
    // =========================================================
    public void Jump()
    {
        if (isLocked)
        {
            return;
        }

        if (!isGrounded)
        {
            return;
        }

        Vector3 v = rb.linearVelocity;
        v.y = 0f;

        rb.linearVelocity = v;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
    }

    // =========================================================
    // DASH
    // =========================================================
    public void StartDash()
    {
        if (isLocked)
        {
            return;
        }

        if (!canDash)
        {
            return;
        }

        if (Mathf.Abs(VerticalInput) < 0.1f &&
            Mathf.Abs(HorizontalInput) < 0.1f)
        {
            return;
        }

        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
    }

    public void HandleDash(PlayerState state)
    {
        if (isLocked)
        {
            return;
        }

        if (state != PlayerState.Dashing)
        {
            return;
        }

        dashTimeLeft -= Time.deltaTime;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 dashDir =
            camForward * VerticalInput +
            camRight * HorizontalInput;

        dashDir.Normalize();

        rb.linearVelocity = dashDir * dashForce;

        if (dashTimeLeft <= 0f)
        {
            isDashing = false;
            dashCooldownTimer = dashCooldown;

            rb.linearVelocity =
                new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    void HandleDashTimers()
    {
        if (canDash)
        {
            return;
        }

        dashCooldownTimer -= Time.deltaTime;

        if (dashCooldownTimer <= 0f)
        {
            canDash = true;
        }
    }

    // =========================================================
    // GROUND CHECK
    // =========================================================
    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}