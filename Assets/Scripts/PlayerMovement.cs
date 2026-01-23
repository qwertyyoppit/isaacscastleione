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

    private Vector3 lookPoint;

    public bool FacesMouse = false;

    private bool moving => movement.sqrMagnitude > 0;

    private InputAction sprintAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        cam = Camera.main;

        sprintAction = input.actions["Sprint"];
        input.actions["Special"].started += _ => FacesMouse = !FacesMouse;
    }

    private void Update()
    {
        movement = input.actions["Move"].ReadValue<Vector2>().normalized;

        var moveDir = new Vector3(movement.x, 0, movement.y);

        var delta = Acceleration * Time.deltaTime * (sprintAction.inProgress ? SprintFactor : 1);
        var rbHorz = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        var target = new Vector3(moveDir.x, 0, moveDir.z) * (Speed * (sprintAction.inProgress ? SprintFactor : 1));
        var velChange = Vector3.MoveTowards(rbHorz, target, delta) - rbHorz;
        // Setting the value directly interferes with physics, so we calculate how much it changes
        rb.linearVelocity += velChange;

        if (moving)
            lookPoint = rb.linearVelocity;
        if (FacesMouse || input.actions["Primary"].inProgress)
        {
            var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out var info, float.MaxValue, LayerMask.GetMask("Ground")))
                lookPoint = (info.point - transform.position).normalized;
        }
        lookPoint.y = 0;
        transform.forward += Vector3.RotateTowards(transform.forward, lookPoint, TurnSpeed * Time.deltaTime, 0.0f) - transform.forward;
    }
}
