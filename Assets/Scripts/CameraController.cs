using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    [SerializeField] float mouseSensitivity = 150f;
    float xRotation = 0f;

    void Start()
    {
        offset = transform.position - player.transform.position;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        HandleMouseLook();
        FollowPlayer();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        player.transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void FollowPlayer()
    {
        transform.position = player.transform.position + offset;
    }
}
