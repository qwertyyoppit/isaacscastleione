using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 100;

    public Action OnDeath;

    private void Awake()
    {
        Health = MaxHealth;
    }

    public void Damage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
