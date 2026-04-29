using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("State")]
    public PlayerState state;

    [Header("References")]
    public PlayerMotor motor;
    public PlayerCamera playerCamera;
    public PlayerAnimator playerAnimator;

    private bool isGrounded;

    void Update()
    {
        // =============================
        // INPUT → MOTOR
        // =============================
        motor.VerticalInput = Input.GetAxis("Vertical");
        motor.CameraForward = transform.forward;
        // =============================
        // CAMERA (INPUT ONLY)
        // =============================
        playerCamera.HandleLook();

        // =============================
        // STATE MACHINE
        // =============================
        HandleStateTransitions();

        // =============================
        // ANIMATION
        // =============================
        playerAnimator.UpdateAnimations(
            motor.VerticalInput,
            playerCamera.TurnInput,
            isGrounded,
            state
        );
    }

    void FixedUpdate()
    {
        // =============================
        // CAMERA ROTATION (SMOOTH YAW)
        // =============================
        playerCamera.UpdateRotation();

        // =============================
        // APPLY ROTATION TO PLAYER
        // =============================
        HandleRotation();

        // =============================
        // MOVEMENT
        // =============================
        motor.HandleMovement(state);

        // =============================
        // DASH
        // =============================
        motor.HandleDash(state);

        // =============================
        // VISUAL ROTATION (MODEL ONLY)
        // =============================
        playerAnimator.HandleVisualRotation(transform);
    }

    // =========================================================
    // ROTATION (PLAYER FOLLOWS CAMERA YAW)
    // =========================================================
    void HandleRotation()
    {
        float targetYaw = playerCamera.CurrentYaw;

        Rigidbody rb = motor.GetComponent<Rigidbody>();
        rb.MoveRotation(Quaternion.Euler(0f, targetYaw, 0f));
    }

    // =========================================================
    // STATE MACHINE
    // =========================================================
    void HandleStateTransitions()
    {
        if (state == PlayerState.Grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                state = PlayerState.Jumping;
                motor.Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                state = PlayerState.Dashing;
                motor.StartDash();
            }
        }

        if (state == PlayerState.Jumping)
        {
            if (isGrounded)
            {
                state = PlayerState.Grounded;
            }
        }

        if (state == PlayerState.Dashing)
        {
            if (!motor.IsDashing)
            {
                state = PlayerState.Grounded;
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