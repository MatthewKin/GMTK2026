using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float sensitivity = 0.02f;

    [Header("Zoom")]
    public Camera cam;
    public float orthoZoom = 3f;
    public float perspectiveZoom = 30f;
    public float zoomSpeed = 8f;

    private float defaultOrtho;
    private float defaultFOV;
    private bool isZoomed;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        defaultOrtho = cam.orthographicSize;
        defaultFOV = cam.fieldOfView;
    }

    void Update()
    {

        // Unlock cursor with Escape
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
        }

        // Relock cursor by clicking
        if (Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
        {
            LockCursor();
        }

        // Camera movement
        Vector2 delta = Mouse.current.delta.ReadValue();
        transform.position += new Vector3(delta.x, delta.y, 0f) * sensitivity;

        // Toggle zoom
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isZoomed = !isZoomed;
            Debug.Log("Zoom toggled: " + isZoomed);
        }

        // Smooth zoom
        if (cam.orthographic)
        {
            float target = isZoomed ? orthoZoom : defaultOrtho;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target, zoomSpeed * Time.deltaTime);
        }
        else
        {
            float target = isZoomed ? perspectiveZoom : defaultFOV;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target, zoomSpeed * Time.deltaTime);
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}