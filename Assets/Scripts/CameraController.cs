using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public CharacterController characterController;
    public float moveSpeed = 10f;

    public float distanceFromPlayer = 5f;
    public Vector3 cameraOffset = new Vector3(0, 2, 0);

    [SerializeField] float mouseSensitivity = 150f;
    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        MovePlayer();
    }

    void LateUpdate()
    {
        UpdateCameraPosition();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 75f);
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer);

        transform.position = player.position + cameraOffset + offset;
        transform.LookAt(player.position + cameraOffset);
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;

        if (moveDirection.magnitude >= 0.1f)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, 0.1f);
        }
    }
}
