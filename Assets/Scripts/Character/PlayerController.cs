using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerState state = PlayerState.Grounded;

    [Header("References")]
    public PlayerMotor motor;
    public PlayerCamera playerCamera;
    public PlayerAnimator playerAnimator;

    private float jumpLockTimer;
    private bool isGrounded;

    void Start()
    {
        motor.cameraTransform = playerCamera.GetCameraTransform();
    }

    void Update()
    {
        // =========================
        // TIMERS
        // =========================
        if (jumpLockTimer > 0f)
            jumpLockTimer -= Time.deltaTime;

        // =========================
        // CAMERA INPUT
        // =========================
        playerCamera.HandleLook();

        // =========================
        // STATE MACHINE
        // =========================
        HandleStateTransitions();

        // =========================
        // ANIMATION INPUT
        // =========================
        playerAnimator.UpdateAnimations(
            motor.VerticalInput,
            playerCamera.TurnInput,
            isGrounded,
            state
        );
    }

    void FixedUpdate()
    {
        // =========================
        // ROTATION (PLAYER FOLLOWS CAMERA)
        // =========================
        transform.rotation = Quaternion.Euler(0f, playerCamera.CurrentYaw, 0f);

        // =========================
        // MOVEMENT
        // =========================
        motor.HandleMovement(state);

        // =========================
        // DASH
        // =========================
        motor.HandleDash(state);

        // =========================
        // VISUAL ROTATION (MODEL ONLY)
        // =========================
        playerAnimator.HandleVisualRotation(transform);
    }

    // =========================================================
    // STATE MACHINE
    // =========================================================
    void HandleStateTransitions()
    {
        // -------- GROUNDED --------
        if (state == PlayerState.Grounded)
        {
            // JUMP
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jumpLockTimer = 0.2f;
                state = PlayerState.Jumping;

                playerAnimator.TriggerJump(); // animation FIRST
                motor.Jump();                // physics SECOND
            }

            // DASH
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                motor.StartDash();

                if (motor.IsDashing) // only change state if dash actually started
                {
                    state = PlayerState.Dashing;
                }
            }
        }

        // -------- JUMPING --------
        if (state == PlayerState.Jumping)
        {
            // prevent instant re-grounding (THIS WAS YOUR BUG)
            if (isGrounded && jumpLockTimer <= 0f)
            {
                state = PlayerState.Grounded;
            }
        }

        // -------- DASHING --------
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