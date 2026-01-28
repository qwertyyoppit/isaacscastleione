using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Entity ent;
    private PlayerInput input;
    private Animator animator;

    public float Knockback;
    public float VerticalKnockback;
    public float Damage;

    [SerializeField] private Transform gripPoint;

    private Hurtbox[] hurtboxes;

    public float AttackDuration;

    private float attackLifetime;

    private InputAction attackAction;

    private void Start()
    {
        ent = GetComponent<Entity>();
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
        if (other.ent.IFrames.CheckImmunity(ent)) return;
        if (attackLifetime <= 0) return;
        ent.Damage(other.ent, Damage, transform.up * VerticalKnockback + transform.forward * Knockback, attackLifetime);
    }
}
