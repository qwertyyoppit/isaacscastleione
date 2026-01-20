using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Action<Hurtbox> OnHurt = delegate { };

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Hurtbox>(out var hurtbox))
        {
            OnHurt?.Invoke(hurtbox);
        }
    }
}
