using UnityEngine;
using System;

public abstract class EnemyBase : MonoBehaviour, IShootable
{
    [field: SerializeField] public int Health { get; private set; }
    public float RemainingHealth => (float)Health / (float)_startHealth;

    private int _startHealth;

    [SerializeField] private EnemyStateManager _state;

    public bool Alive => Health > 0;

    public Action OnDeath = default;
    public Action<int> OnDamageTaken = default;
    public Action<int> OnHealthGained = default;


    private void Start()
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

    public void OnShot(Vector3 dir, AmmunitionData data) // interface method
    {
        if (!Alive) return;
        TakeDamage(data.Damage);
        _state.OnShotTaken(data.Damage);
    }
}
