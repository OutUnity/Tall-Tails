using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform visual;
    [SerializeField] private float visualTurnSmooth = 10f;

    private float currentTurn;

    public void UpdateAnimations(float vertical, float turnInput, bool isGrounded, PlayerState state)
    {
        float speed = 0f;

        if (Mathf.Abs(vertical) > 0.1f)
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

        currentTurn = Mathf.Lerp(currentTurn, turnInput, Time.deltaTime * 6f);

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", currentTurn, 0.1f, Time.deltaTime);
        animator.SetBool("Grounded", isGrounded);

        if (state == PlayerState.Jumping)
        {
            animator.SetTrigger("Jump");
        }
    }
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
    public void HandleVisualRotation(Transform player)
    {
        Vector3 currentForward = visual.forward;
        Vector3 targetForward = player.forward;

        currentForward.y = 0f;
        targetForward.y = 0f;

        currentForward.Normalize();
        targetForward.Normalize();

        Vector3 newDir = Vector3.Slerp(currentForward, targetForward, Time.deltaTime * visualTurnSmooth);
        visual.rotation = Quaternion.LookRotation(newDir);
    }
}