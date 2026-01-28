using UnityEngine;

public class Entity : MonoBehaviour
{
    public Faction Faction;

    public Rigidbody Rb;

    public IFramesManager IFrames;

    public HealthComponent Health;

    private void Awake()
    {
        if (!TryGetComponent<Rigidbody>(out var RbComp))
        {
            RbComp = gameObject.AddComponent<Rigidbody>();
        }
        Rb = RbComp;
        if (!TryGetComponent<IFramesManager>(out var IFComp))
        {
            IFComp = gameObject.AddComponent<IFramesManager>();
        }
        IFrames = IFComp;
        if (!TryGetComponent<HealthComponent>(out var healthComp))
        {
            healthComp = gameObject.AddComponent<HealthComponent>();
        }
        Health = healthComp;

        if (!TryGetComponent<Hitbox>(out var _))
            gameObject.AddComponent<Hitbox>();
    }

    public void Damage(Entity target, float damage, Vector3 knockback, float iFrameDuration, IFrameType iframeType = IFrameType.Instance)
    {
        target.Health.Damage(damage);
        target.Rb.AddForce(knockback, ForceMode.Impulse);
        target.IFrames.InflictEntityIFrames(this, iFrameDuration);
    }
}
