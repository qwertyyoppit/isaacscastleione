using System;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    //public Entity entity or
    //public Player player
    //for refs to classes like Health, Stats etc.

    public Action<Hitbox> OnHit = delegate { };

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Hitbox>(out var hitbox))
        {
            OnHit?.Invoke(hitbox);
        }    
    }
}
