using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform player;   // IMPORTANT: assign Player root here

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookUp = 60f;
    [SerializeField] private float maxLookDown = -20f;
    [SerializeField] private float smooth = 0.08f;

    private float yaw;                 // ONLY THIS SCRIPT OWNS YAW
    private float targetPitch;
    private float currentPitch;
    private float pitchVel;

    public float CurrentYaw => yaw;
    public float TurnInput { get; private set; }

    public Transform GetCameraTransform()
    {
        return cameraPivot;
    }

    void Start()
    {
        yaw = player.eulerAngles.y;
    }
    public void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        TurnInput = mouseX;

        // =========================
        // YAW (PLAYER ROTATION OWNER)
        // =========================
        yaw += mouseX;

        // Apply yaw directly to player
        //player.rotation = Quaternion.Euler(0f, yaw, 0f);

        // =========================
        // PITCH (CAMERA ONLY)
        // =========================
        targetPitch -= mouseY;
        targetPitch = Mathf.Clamp(targetPitch, maxLookDown, maxLookUp);

        currentPitch = Mathf.SmoothDamp(
            currentPitch,
            targetPitch,
            ref pitchVel,
            smooth
        );

        cameraPivot.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);
    }
}