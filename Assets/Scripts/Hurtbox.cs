using System;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Entity ent;

    public Action<Hitbox> OnHit = delegate { };

    private void Start()
    {
        if (!ent) ent = GetComponentInParent<Entity>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<Hitbox>(out var hitbox)) return;
        if (hitbox.ent.Faction == ent.Faction) return;
        OnHit?.Invoke(hitbox);
    }
}
