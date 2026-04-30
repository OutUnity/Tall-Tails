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

    public Transform cameraTransform;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }

    public bool IsDashing => isDashing;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");

        HandleDashTimers();
    }

    // =========================================================
    // MOVEMENT (CLEAN CAMERA-RELATIVE)
    // =========================================================
    public void HandleMovement(PlayerState state)
    {
        if (state != PlayerState.Grounded || isDashing)
            return;

        // CAMERA DIRECTIONS (ONLY SOURCE OF MOVEMENT DIRECTION)
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

        Vector3 velocity = moveDirection * speed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    // =========================================================
    // JUMP
    // =========================================================
    public void Jump()
    {
        if (!isGrounded)
            return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
    }

    // =========================================================
    // DASH
    // =========================================================
    public void StartDash()
    {
        if (!canDash)
            return;

        // 🚫 BLOCK DASH IF NO INPUT
        if (Mathf.Abs(VerticalInput) < 0.1f && Mathf.Abs(HorizontalInput) < 0.1f)
            return;

        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
    }

    public void HandleDash(PlayerState state)
    {
        if (state != PlayerState.Dashing)
            return;

        dashTimeLeft -= Time.deltaTime;

        // USE INPUT DIRECTION (NOT JUST CAMERA FORWARD)
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

            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    void HandleDashTimers()
    {
        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;

            if (dashCooldownTimer <= 0f)
            {
                canDash = true;
            }
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