using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ChaserAI : MonoBehaviour
{
    public float Acceleration;
    public float Speed;
    public float TurnSpeed;

    public float Knockback;

    public float AttackDuration;

    [SerializeField] private float attackLifetime;

    private Rigidbody rb;
    [SerializeField] private Transform gripPoint;
    private Animator attackAnimator;

    public Transform player;

    private Hurtbox[] hurtboxes;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackAnimator = gripPoint.GetComponent<Animator>();

        hurtboxes = GetComponentsInChildren<Hurtbox>();
        foreach (var hurtbox in hurtboxes)
        {
            hurtbox.OnHit += OnHit;
        }
    }

    private void Attack()
    {
        attackLifetime = AttackDuration;
        attackAnimator.SetTrigger("attack");
    }

    private void Update()
    {
        var playerDir = (player.position - transform.position).normalized;
        var playerDist = (player.position - transform.position).magnitude;

        attackLifetime = Mathf.Max(0, attackLifetime - Time.deltaTime);
        if (attackLifetime <= 0)
        {
            attackAnimator.ResetTrigger("attack");
        }
        else return;
        Vector3 moveDir;
        if (playerDist > 2)
        {
            moveDir = playerDir;
        }
        else
        {
            moveDir = Vector3.zero;
            Attack();
        }

        var delta = Acceleration * Time.deltaTime;
        var rbHorz = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        var target = new Vector3(moveDir.x, 0, moveDir.z) * Speed;
        var change = Vector3.MoveTowards(rbHorz, target, delta) - rbHorz;

        rb.linearVelocity += change;

        rbHorz = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        transform.forward = Vector3.RotateTowards(transform.forward, rbHorz, TurnSpeed * Time.deltaTime, 0.0f);
    }

    private void OnHit(Hitbox other)
    {
        if (attackLifetime <= 0) return;
        if (other.TryGetComponent<Rigidbody>(out var hitRb))
        {
            hitRb.AddForce(transform.up * (0.6f * Knockback) + transform.forward * Knockback, ForceMode.Impulse);
            attackLifetime = 0;
        }
    }
}
