using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ChaserAI : MonoBehaviour
{
    public float Speed;

    public float Knockback;

    public float AttackDuration;

    private float attackLifetime;

    private Rigidbody rb;
    private Transform gripPoint;
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

        transform.forward = playerDir;

        attackLifetime = Mathf.Max(0, attackLifetime - Time.deltaTime);
        if (attackLifetime <= 0)
        {
            attackAnimator.ResetTrigger("attack");
        }
        else return;
        if (playerDist > 1)
        {
            rb.linearVelocity = playerDir * (Speed * Time.deltaTime);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            Attack();
        }
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
