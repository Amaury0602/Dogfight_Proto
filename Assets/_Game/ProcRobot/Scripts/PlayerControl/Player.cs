using UnityEngine;
using System;

public class Player : MonoBehaviour, IShootable
{
    [field: SerializeField] public int Health { get; private set; }

    public bool Alive => Health > 0;

    public Action OnTakeDamage = default;
    public Action OnDeath = default;

    public void OnShot(Vector3 dir, AmmunitionData data)
    {
        if (!Alive) return;
        OnTakeDamage?.Invoke();

        Health -= data.Damage;

        if (Health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        OnDeath?.Invoke();
    }
}
