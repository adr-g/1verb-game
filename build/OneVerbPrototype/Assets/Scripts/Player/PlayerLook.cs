using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensitivity = 0.1f;

    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isLocked = Cursor.lockState == CursorLockMode.Locked;

            Cursor.lockState = isLocked
                ? CursorLockMode.None
                : CursorLockMode.Locked;

            Cursor.visible = isLocked;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (Mouse.current == null) return;

        Vector2 mouseMovement = Mouse.current.delta.ReadValue();

        float mouseX = mouseMovement.x * mouseSensitivity;
        float mouseY = mouseMovement.y * mouseSensitivity;

        playerBody.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}