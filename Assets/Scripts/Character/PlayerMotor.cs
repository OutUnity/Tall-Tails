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
    public Vector3 CameraForward { get; set; }

    private bool isDashing;
    private bool canDash = true;
    private float dashTimer;
    private float cooldownTimer;

    // =============================
    // INPUT VALUES (SET BY CONTROLLER)
    // =============================
    public float VerticalInput { get; set; }

    // =============================
    // STATE OUTPUT
    // =============================
    public bool IsDashing
    {
        get { return isDashing; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // =========================================================
    // MOVEMENT (CALLED FROM CONTROLLER)
    // =========================================================
    public void HandleMovement(PlayerState state)
    {
        if (state != PlayerState.Grounded)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }

        Vector3 forward = CameraForward;
        forward.y = 0f;
        forward.Normalize();

        float speed = moveSpeed;

        if (VerticalInput > 0f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed *= runMultiplier;
            }
        }

        Vector3 velocity = forward * VerticalInput * speed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    // =========================================================
    // JUMP (CALLED FROM CONTROLLER)
    // =========================================================
    public void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // =========================================================
    // DASH START (CALLED FROM CONTROLLER)
    // =========================================================
    public void StartDash()
    {
        if (!canDash)
        {
            return;
        }

        isDashing = true;
        canDash = false;
        dashTimer = dashDuration;
    }

    // =========================================================
    // DASH UPDATE (CALLED FROM CONTROLLER)
    // =========================================================
    public void HandleDash(PlayerState state)
    {
        if (state != PlayerState.Dashing)
        {
            HandleDashCooldown();
            return;
        }

        dashTimer -= Time.deltaTime;

        Vector3 forward = CameraForward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 dashVelocity = forward * dashForce;
        dashVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = dashVelocity;

        if (dashTimer <= 0f)
        {
            isDashing = false;
            cooldownTimer = dashCooldown;
        }
    }

    // =========================================================
    // DASH COOLDOWN
    // =========================================================
    private void HandleDashCooldown()
    {
        if (!canDash)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                canDash = true;
            }
        }
    }
}