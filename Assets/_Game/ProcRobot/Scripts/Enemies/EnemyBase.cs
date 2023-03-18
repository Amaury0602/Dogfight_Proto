using UnityEngine;
using System;

public abstract class EnemyBase : MonoBehaviour, IShootable
{
    [field: SerializeField] public int Health { get; private set; }
    public float RemainingHealth => (float)Health / (float)_startHealth;

    private int _startHealth;

    [SerializeField] private EnemyStateManager _state;

    [field: SerializeField] public Squad Squad { get; set; }

    public bool Alive => Health > 0;

    public Action<EnemyBase> OnDeath = default;
    public Action<int> OnDamageTaken = default;
    public Action<int> OnHealthGained = default;


    [SerializeField] protected GameObject _lockableObject;

    [SerializeField] private CapsuleCollider _mainCollider;

    [SerializeField] private ParticleSystem _deathFX;


    private void Start()
    {
        _startHealth = Health;
        Squad.AttackAlert += OnAttackAlert;
    }

    private void OnAttackAlert()
    {
        _state.SetState(_state.AttackState);
    }

    protected virtual void Die()
    {
        if (_mainCollider) _mainCollider.enabled = false;
        if (_deathFX) _deathFX.Play();
        _lockableObject.SetActive(false);
        OnDeath?.Invoke(this);
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

    public void OnShot(Vector3 dir, int damage, AmmunitionData data) // interface method
    {
        if (!Alive) return;
        TakeDamage(damage);
        _state.OnShotTaken(damage);
    }
}
