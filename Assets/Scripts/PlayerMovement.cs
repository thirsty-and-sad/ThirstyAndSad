using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public Transform playerModel;
    public float walkingSpeed = 5.0f;
    public float sprintingSpeed = 10.0f;
    public float acceleration = 15.0f;
    public float deceleration = 20.0f;
    InputAction moveAction;
    InputAction sprintAction;
    InputAction jumpAction;
    Vector3 movement_input;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void FixedUpdate()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 directional_input = new Vector3(moveInput.x, 0.0f, moveInput.y);
        
        if (directional_input.AlmostZero())
        {
            movement_input = Vector3.MoveTowards(movement_input, Vector3.zero, deceleration * Time.deltaTime);
        } 
        else
        {
            playerModel.forward = directional_input;
            Vector3 target = directional_input * get_current_speed();
            movement_input = Vector3.MoveTowards(movement_input, target, acceleration * Time.deltaTime);
        }

        float up_vel = characterController.velocity.y;

        if (!characterController.isGrounded) {
            up_vel -= 9.81f * Time.deltaTime;
        } else {
            up_vel = 0.0f;
        }
        
        if (jumpAction.IsPressed() && characterController.isGrounded) {
            up_vel += 3.0f;
        }

        characterController.Move((movement_input + Vector3.up * up_vel) * Time.deltaTime);



    }

    float get_current_speed()
    {
        if (sprintAction.IsPressed()) {
            return sprintingSpeed;
        }
        return walkingSpeed;
    }
}
