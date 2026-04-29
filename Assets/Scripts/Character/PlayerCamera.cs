using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraHolder;

    [Header("Camera")]
    [SerializeField] private float cameraSmooth = 0.1f;

    [Header("Camera Limits")]
    [SerializeField] private float maxLookUp = 60f;
    [SerializeField] private float maxLookDown = -20f;

    [Header("Camera Feel")]
    [SerializeField] private float cameraSoftness = 5f;

    [Header("Hybrid Turning")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float edgeTurnStrength = 120f;
    [SerializeField] private float edgeExponent = 1.5f;

    [Header("Recenter")]
    [SerializeField] private float recenterSpeed = 2f;
    [SerializeField] private float recenterDeadZone = 0.1f;

    [Header("Camera Follow Lag")]
    [SerializeField] private float cameraFollowSmooth = 5f;

    // =============================
    // OUTPUT VALUES (USED BY CONTROLLER)
    // =============================
    public float CurrentYaw { get; private set; }
    public float TurnInput { get; private set; }

    // =============================
    // INTERNAL STATE
    // =============================
    private float xRotation = 0f;
    private float targetXRotation = 0f;
    private float cameraRotVelocity;

    private float targetYRotation;
    private float currentYRotation;

    // =========================================================
    // CALLED FROM PlayerController (NOT Update)
    // =========================================================
    public void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        Vector2 mousePos = Input.mousePosition;

        float centerX = Screen.width * 0.5f;
        float centerY = Screen.height * 0.5f;

        float offsetX = (mousePos.x - centerX) / centerX;
        float offsetY = (mousePos.y - centerY) / centerY;

        float edgeX = Mathf.Sign(offsetX) * Mathf.Pow(Mathf.Abs(offsetX), edgeExponent);
        float edgeY = Mathf.Sign(offsetY) * Mathf.Pow(Mathf.Abs(offsetY), edgeExponent);

        float finalTurn = mouseX + (edgeX * edgeTurnStrength * Time.deltaTime);
        float finalLookY = mouseY + (edgeY * edgeTurnStrength * Time.deltaTime);

        // OUTPUT FOR ANIMATOR
        TurnInput = finalTurn;

        // STORE ROTATION (NOT APPLYING TO PLAYER HERE)
        targetYRotation += finalTurn;
        targetXRotation -= finalLookY;

        // =============================
        // RECENTER
        // =============================
        if (Mathf.Abs(offsetX) < recenterDeadZone)
        {
            TurnInput = Mathf.Lerp(TurnInput, 0f, Time.deltaTime * recenterSpeed);
        }

        if (Mathf.Abs(offsetY) < recenterDeadZone)
        {
            float centerTarget = Mathf.Clamp(targetXRotation, maxLookDown, maxLookUp);
            targetXRotation = Mathf.Lerp(targetXRotation, centerTarget, Time.deltaTime * recenterSpeed);
        }

        // =============================
        // SOFT LIMITS
        // =============================
        if (targetXRotation > maxLookUp)
        {
            float excess = targetXRotation - maxLookUp;
            targetXRotation -= excess * Time.deltaTime * cameraSoftness;
        }
        else if (targetXRotation < maxLookDown)
        {
            float excess = maxLookDown - targetXRotation;
            targetXRotation += excess * Time.deltaTime * cameraSoftness;
        }

        // =============================
        // SMOOTH CAMERA PITCH
        // =============================
        xRotation = Mathf.SmoothDamp(
            xRotation,
            targetXRotation,
            ref cameraRotVelocity,
            cameraSmooth
        );

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // =========================================================
    // CALLED FROM PlayerController (FixedUpdate)
    // =========================================================
    public void UpdateRotation()
    {
        currentYRotation = Mathf.Lerp(
            currentYRotation,
            targetYRotation,
            Time.fixedDeltaTime * cameraFollowSmooth
        );

        // OUTPUT FOR CONTROLLER (player rotation happens there)
        CurrentYaw = currentYRotation;
    }
}