using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
<<<<<<< HEAD
    public Vector2 mouseSensitivity = Vector2.one;
    public Transform playerHead;
    float xRotation, yRotation;
    InputAction lookAction;
=======
    public Transform player;
    public CharacterController characterController;
    public float moveSpeed = 10f;
>>>>>>> 42de92a17aaf76e81b325cdc2dec127dfa4e9e55

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
        playerHead.localRotation = Quaternion.Euler(-yRotation, xRotation, 0);
    }
}
