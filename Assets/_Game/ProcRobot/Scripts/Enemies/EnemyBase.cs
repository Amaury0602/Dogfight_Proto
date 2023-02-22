using UnityEngine;
using System;

public abstract class EnemyBase : MonoBehaviour
{
    [field: SerializeField] public int Health { get; private set; }
    public float RemainingHealth => (float)_startHealth / (float)Health;

    private int _startHealth;

    public bool Alive => Health > 0;

    public Action OnDeath = default;
    public Action<int> OnDamageTaken = default;
    public Action<int> OnHealthGained = default;


    private void Awake()
    {
        _startHealth = Health;
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();
    }

    public virtual void TakeDamage(int damage, AmmunitionEffect effect = AmmunitionEffect.None)
    {
        if (!Alive) return;

        OnDamageTaken?.Invoke(damage);

        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void OnShot(Vector3 dir, AmmunitionData data)
    {
        TakeDamage(data.Damage);
    }
}
