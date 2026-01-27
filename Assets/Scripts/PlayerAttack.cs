using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInput input;
    private Animator animator;

    [SerializeField] private Transform gripPoint;

    private Hurtbox[] hurtboxes;

    public float AttackDuration;

    private float attackLifetime;

    private InputAction attackAction;

    private void Start()
    {
        input = FindFirstObjectByType<PlayerInput>();
        animator = gripPoint.GetComponent<Animator>();

        attackAction = input.actions["Primary"];

        hurtboxes = gripPoint.GetComponentsInChildren<Hurtbox>();

        foreach (var hurtbox in hurtboxes)
        {
            hurtbox.OnHit += OnHit;
        }
    }

    private void Attack()
    {
        attackLifetime = AttackDuration;
        animator.SetBool("attack", true);
    }

    private void Update()
    {
        attackLifetime = Mathf.Max(0, attackLifetime - Time.deltaTime);
        animator.SetFloat("attackPoint", Mathf.Abs(attackLifetime - AttackDuration) / AttackDuration);
        if (attackLifetime == 0)
        {
            animator.SetBool("attack", false);
            if (attackAction.inProgress)
                Attack();
        }
    }

    private void OnHit(Hitbox other)
    {
        if (attackLifetime <= 0) return;
        if (other.TryGetComponent<Rigidbody>(out var hitRb))
        {
            hitRb.AddForce(transform.up * 3 + transform.forward * 5, ForceMode.Impulse);
            attackLifetime = 0;
        }
    }
}
