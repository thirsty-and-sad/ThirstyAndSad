using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Vector2 mouseSensitivity = Vector2.one;
    public Transform playerHead;
    float xRotation, yRotation;
    InputAction lookAction;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lookAction = InputSystem.actions.FindAction("Look");
    }

    private void Update()
    {
        Vector2 mouse_input = lookAction.ReadValue<Vector2>() * mouseSensitivity;
        xRotation += mouse_input.x;
        yRotation += mouse_input.y;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        playerHead.localRotation = Quaternion.Euler(yRotation, xRotation, 0);
    }
}
