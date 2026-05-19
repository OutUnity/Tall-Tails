using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerState state = PlayerState.Grounded;

    [Header("References")]
    public PlayerMotor motor;
    public PlayerCamera playerCamera;
    public PlayerAnimator playerAnimator;

    [Header("State Flags")]
    public bool isLoading = false;

    private float jumpLockTimer;
    private bool isGrounded;

    // =========================================================
    // INIT
    // =========================================================
    void OnEnable()
    {
        SetLoadingState(false);
    }

    void Start()
    {
        if (motor != null && playerCamera != null)
        {
            motor.cameraTransform = playerCamera.GetCameraTransform();
        }
    }

    // =========================================================
    // LOADING CONTROL
    // =========================================================
    public void SetLoadingState(bool loading)
    {
        isLoading = loading;

        if (loading)
        {
            state = PlayerState.Grounded;
        }
    }

    // =========================================================
    // UPDATE
    // =========================================================
    void Update()
    {
        if (isLoading)
        {
            return;
        }

        if (jumpLockTimer > 0f)
        {
            jumpLockTimer -= Time.deltaTime;
        }

        playerCamera.HandleLook();

        HandleStateTransitions();

        playerAnimator.UpdateAnimations(
            motor.VerticalInput,
            playerCamera.TurnInput,
            isGrounded,
            state
        );
    }

    // =========================================================
    // FIXED UPDATE
    // =========================================================
    void FixedUpdate()
    {
        if (isLoading)
        {
            return;
        }

        transform.rotation =
            Quaternion.Euler(0f, playerCamera.CurrentYaw, 0f);

        motor.HandleMovement(state);
        motor.HandleDash(state);

        playerAnimator.HandleVisualRotation(transform);
    }

    // =========================================================
    // STATE MACHINE
    // =========================================================
    void HandleStateTransitions()
    {
        if (isLoading)
        {
            return;
        }

        if (state == PlayerState.Grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jumpLockTimer = 0.2f;
                state = PlayerState.Jumping;

                playerAnimator.TriggerJump();
                motor.Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                motor.StartDash();

                if (motor.IsDashing)
                {
                    state = PlayerState.Dashing;
                }
            }
        }

        if (state == PlayerState.Jumping)
        {
            if (isGrounded && jumpLockTimer <= 0f)
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