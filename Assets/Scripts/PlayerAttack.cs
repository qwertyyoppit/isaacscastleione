using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInput input;
    private Animator animator;

    [SerializeField] private BoxCollider hurtBox;

    public float AttackDuration;

    private float attackLifetime;

    private InputAction attackAction;

    private void Start()
    {
        input = FindFirstObjectByType<PlayerInput>();
        animator = GetComponent<Animator>();

        attackAction = input.actions["Primary"];
    }

    private void Attack()
    {
        attackLifetime = AttackDuration;
        hurtBox.enabled = true;
        animator.SetTrigger("attack");
    }

    private void Update()
    {
        attackLifetime = Mathf.Max(0, attackLifetime - Time.deltaTime);
        if (attackLifetime == 0)
        {
            animator.ResetTrigger("attack");
            hurtBox.enabled = false;
            if (attackAction.inProgress)
                Attack();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        if (other.TryGetComponent<Rigidbody>(out var hitRb))
        {
            hitRb.AddForce(transform.up * 3 + transform.forward * 5, ForceMode.Impulse);
            other.gameObject.name += " (violated)";
            attackLifetime = 0;
        }
    }
}
