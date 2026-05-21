using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform player;

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookUp = 60f;
    [SerializeField] private float maxLookDown = -20f;
    [SerializeField] private float smooth = 0.08f;

    private float yaw;
    private float targetPitch;
    private float currentPitch;
    private float pitchVel;

    public float CurrentYaw => yaw;
    public float TurnInput { get; private set; }

    public Transform GetCameraTransform()
    {
        return cameraPivot;
    }

    void Awake()
    {
        // SAFETY: auto-assign pivot if missing
        if (cameraPivot == null)
        {
            cameraPivot = transform;
        }

        if (player == null)
        {
            Debug.LogError("PlayerCamera: Player reference is missing!");
        }
    }

    void Start()
    {
        if (player != null)
        {
            yaw = player.eulerAngles.y;
        }
        else
        {
            yaw = transform.eulerAngles.y;
        }

        targetPitch = 0f;
        currentPitch = 0f;
    }

    public void HandleLook()
    {
        if (player == null || cameraPivot == null)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // SAFETY: prevent NaN from input spikes
        if (float.IsNaN(mouseX) || float.IsNaN(mouseY))
        {
            mouseX = 0f;
            mouseY = 0f;
        }

        TurnInput = mouseX;

        // =========================
        // YAW (PLAYER ROTATION)
        // =========================
        yaw += mouseX;

        if (float.IsNaN(yaw))
        {
            yaw = 0f;
        }

        player.rotation = Quaternion.Euler(0f, yaw, 0f);

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

        // SAFETY CHECK BEFORE APPLYING ROTATION
        if (float.IsNaN(currentPitch))
        {
            currentPitch = 0f;
        }

        Quaternion rot = Quaternion.Euler(currentPitch, 0f, 0f);

        if (float.IsNaN(rot.x) || float.IsNaN(rot.y) || float.IsNaN(rot.z) || float.IsNaN(rot.w))
        {
            rot = Quaternion.identity;
        }

        cameraPivot.localRotation = rot;
    }
}