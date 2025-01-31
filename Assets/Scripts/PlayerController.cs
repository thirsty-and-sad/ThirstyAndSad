using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform playerModel;
    public Transform playerHead;
    public float walkingSpeed = 5.0f;
    public float sprintingSpeed = 10.0f;
    public float acceleration = 15.0f;
    public float deceleration = 20.0f;
    InputAction moveAction;
    InputAction sprintAction;
    InputAction jumpAction;
    Vector3 movement_vector;
    float movementFactor;
    public Animator animator;


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
        float current_speed = get_current_speed();

        if (directional_input.AlmostZero())
        {
            movement_vector = Vector3.MoveTowards(movement_vector, Vector3.zero, deceleration * Time.deltaTime);
        } 
        else
        {
            Vector3 target = directional_input * current_speed;
            target = Quaternion.AngleAxis(playerHead.eulerAngles.y, Vector3.up) * target;
            playerModel.forward = target.normalized;
            movement_vector = Vector3.MoveTowards(movement_vector, target, acceleration * Time.deltaTime);
        }

        float up_vel = characterController.velocity.y;

        if (!characterController.isGrounded) {
            up_vel -= 9.81f * Time.deltaTime;
        } else {
            up_vel = 0.0f;
        }
        
        if (jumpAction.IsPressed() && characterController.isGrounded) {
            up_vel += 4.0f;
            animator.SetTrigger("jump");
        }

        characterController.Move((movement_vector + Vector3.up * up_vel) * Time.deltaTime);

        movementFactor = movement_vector.magnitude / current_speed;
        animator.SetFloat("movementFactor", movementFactor);
        animator.SetBool("isMoving", !Mathf.Approximately(0.0f, movementFactor));
        animator.SetBool("isFalling", characterController.velocity.y < 0.0f);
        animator.SetBool("isGrounded", characterController.isGrounded);
    }

    float get_current_speed()
    {
        if (sprintAction.IsPressed()) {
            return sprintingSpeed;
        }
        return walkingSpeed;
    }
}
