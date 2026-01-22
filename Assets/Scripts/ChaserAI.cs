using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ChaserAI : MonoBehaviour
{
    public float Speed;

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

        transform.forward = playerDir;

        attackLifetime = Mathf.Max(0, attackLifetime - Time.deltaTime);
        if (attackLifetime <= 0)
        {
            attackAnimator.ResetTrigger("attack");
        }
        else return;
        if (playerDist > 2)
        {
            Debug.Log("ack");
            rb.linearVelocity = playerDir * (Speed) + new Vector3(0, rb.linearVelocity.y, 0);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
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
