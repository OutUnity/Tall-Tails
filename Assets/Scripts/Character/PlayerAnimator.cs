using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visual;
    [SerializeField] private Animator animator;

    [Header("Visual Rotation")]
    [SerializeField] private float visualTurnSmooth = 10f;

    // =========================================================
    // CALLED FROM PlayerController (Update)
    // =========================================================
    public void UpdateAnimations(
        float verticalInput,
        float turnInput,
        bool isGrounded,
        PlayerState state
    )
    {
        // ---------------- SPEED CALCULATION ----------------
        float speed = 0f;

        if (Mathf.Abs(verticalInput) < 0.1f)
        {
            speed = 0f;
        }
        else
        {
            if (verticalInput > 0f)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = 1f;        // Run
                }
                else
                {
                    speed = 0.5f;      // Walk
                }
            }
            else
            {
                speed = -0.5f;         // Backpedal
            }
        }

        // ---------------- TURN SMOOTHING ----------------
        float currentTurn = animator.GetFloat("Turn");

        float smoothedTurn = Mathf.Lerp(
            currentTurn,
            turnInput,
            Time.deltaTime * 10f
        );

        // ---------------- SET PARAMETERS ----------------
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", smoothedTurn, 0.1f, Time.deltaTime);
        animator.SetBool("Grounded", isGrounded);

        // Optional: Trigger jump animation via state
        if (state == PlayerState.Jumping)
        {
            animator.SetTrigger("Jump");
        }
    }

    // =========================================================
    // CALLED FROM PlayerController (FixedUpdate)
    // =========================================================
    public void HandleVisualRotation(Transform playerTransform)
    {
        Vector3 currentForward = visual.forward;
        Vector3 targetForward = playerTransform.forward;

        currentForward.y = 0f;
        targetForward.y = 0f;

        currentForward.Normalize();
        targetForward.Normalize();

        if (currentForward != targetForward)
        {
            Vector3 newDirection = Vector3.Slerp(
                currentForward,
                targetForward,
                visualTurnSmooth * Time.fixedDeltaTime
            );

            visual.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}