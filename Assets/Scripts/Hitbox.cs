using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Entity ent;

    public Action<Hurtbox> OnHurt = delegate { };

    private void Start()
    {
        if (!ent) ent = GetComponentInParent<Entity>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<Hurtbox>(out var hurtbox)) return;
        if (hurtbox.ent.Faction == ent.Faction) return;
        OnHurt?.Invoke(hurtbox);
    }
}
