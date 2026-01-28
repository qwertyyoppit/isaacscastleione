using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class ChaserAI : MonoBehaviour
{
    public float Acceleration;
    public float Speed;
    public float TurnSpeed;

    public float Knockback;
    public float VerticalKnockback;
    public float Damage;

    public float AttackDuration;

    [SerializeField] private float attackLifetime;
    [SerializeField] private float maxTargetDistance;

    private Rigidbody rb;
    [SerializeField] private Transform gripPoint;
    private Animator attackAnimator;
    private Entity ent;

    private Transform chaseTarget;

    private Hurtbox[] hurtboxes;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackAnimator = gripPoint.GetComponent<Animator>();
        ent = GetComponent<Entity>();

        hurtboxes = GetComponentsInChildren<Hurtbox>();
        foreach (var hurtbox in hurtboxes)
        {
            hurtbox.OnHit += OnHit;
        }

        StartCoroutine(FindTargetAtRate(0.25f));
    }

    public IEnumerator FindTargetAtRate(float rate)
    {
        while (true)
        {
            FindTarget();
            yield return new WaitForSeconds(rate);
        }
    }

    private void Attack()
    {
        attackLifetime = AttackDuration;
        attackAnimator.SetTrigger("attack");
    }

    private void FindTarget()
    {
        Transform closest = null;
        foreach (var found in Physics.OverlapSphere(transform.position, maxTargetDistance))
        {
            if (!found.TryGetComponent<Entity>(out var foundEnt)) continue; //fent
            if (ent == foundEnt) continue;
            if (!ent.Faction.Attacks(foundEnt.Faction)) continue;

            if (closest && Vector3.Distance(transform.position, closest.position) < Vector3.Distance(transform.position, found.transform.position)) continue;
            closest = found.transform;
        }
        chaseTarget = closest;
    }

    private void Update()
    {
        var targetDir = ((chaseTarget ? chaseTarget : transform).position - transform.position).normalized;
        var targetDist = ((chaseTarget ? chaseTarget : transform).position - transform.position).magnitude;

        attackLifetime = Mathf.Max(0, attackLifetime - Time.deltaTime);
        attackAnimator.SetFloat("attackPoint", Mathf.Abs(attackLifetime - AttackDuration) / AttackDuration);
        if (attackLifetime <= 0)
        {
            attackAnimator.ResetTrigger("attack");
        }
        else return;
        Vector3 moveDir;
        if (targetDist > 2)
        {
            moveDir = targetDir;
        }
        else
        {
            moveDir = Vector3.zero;
            Attack();
        }

        var delta = Acceleration * Time.deltaTime;
        var rbHorz = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        var moveTarget = new Vector3(moveDir.x, 0, moveDir.z) * Speed;
        var change = Vector3.MoveTowards(rbHorz, moveTarget, delta) - rbHorz;

        rb.linearVelocity += change;

        rbHorz = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        transform.forward = Vector3.RotateTowards(transform.forward, rbHorz, TurnSpeed * Time.deltaTime, 0.0f);
    }

    private void OnHit(Hitbox other)
    {
        if (other.ent.IFrames.CheckImmunity(ent)) return;
        if (attackLifetime <= 0) return;
        ent.Damage(other.ent, Damage, transform.up * VerticalKnockback + transform.forward * Knockback, attackLifetime);
    }
}
