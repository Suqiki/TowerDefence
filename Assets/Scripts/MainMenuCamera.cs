using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [Header("Idle Movement")]
    public float idleAmountX = 0.15f;
    public float idleAmountY = 0.08f;

    public float idleSpeedX = 0.3f;
    public float idleSpeedY = 0.2f;

    [Header("Mouse Rotation")]
    public float rotationAmountX = 3f;
    public float rotationAmountY = 2f;

    public float smoothness = 5f;

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        float time = Time.time;

        // =========================
        // IDLE FLOATING POSITION
        // =========================

        Vector3 idleOffset = new Vector3(
            Mathf.Sin(time * idleSpeedX) * idleAmountX,
            Mathf.Sin(time * idleSpeedY) * idleAmountY,
            0f
        );

        Vector3 targetPosition = startPos + idleOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * smoothness
        );

        // =========================
        // MOUSE LOOK ROTATION
        // =========================

        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f);
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f);

        float rotY = mouseX * rotationAmountX;
        float rotX = -mouseY * rotationAmountY;

        Quaternion targetRotation =
            startRot *
            Quaternion.Euler(rotX, rotY, 0);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothness
        );
    }
}