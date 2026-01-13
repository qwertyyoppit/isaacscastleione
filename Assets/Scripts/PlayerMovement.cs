using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Camera cam;
    private PlayerInput input;

    public float Acceleration;
    public float Speed;
    public float SprintFactor;
    public float TurnSpeed;

    private Vector2 movement;

    private bool moving => movement.sqrMagnitude > 0;

    private InputAction sprintAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        cam = Camera.main;

        sprintAction = input.actions["Sprint"];
    }

    private void Update()
    {
        movement = input.actions["Move"].ReadValue<Vector2>().normalized;

        var moveDir = new Vector3(movement.x, 0, movement.y);

        var delta = Acceleration * (sprintAction.inProgress ? SprintFactor : 1);

        var target = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z) * (Speed * (sprintAction.inProgress ? SprintFactor : 1));

        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, target, delta * Time.deltaTime);

        var rbMove = rb.linearVelocity;
        rbMove.y = 0;
        transform.forward = Vector3.RotateTowards(transform.forward, rbMove, TurnSpeed * Time.deltaTime, 0.0f);
    }
}
