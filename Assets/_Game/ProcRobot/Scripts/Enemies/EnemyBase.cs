using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IShootable
{
    [SerializeField]
    protected Transform _currentTarget;
    protected WeaponBase _currentWeapon;

    [Header("Movement variables")]
    [SerializeField]
    protected float _moveSpeed;
    [SerializeField]
    protected float _rotationSpeed;

    [field: SerializeField] public int Health { get; private set; }

    public bool Alive => Health > 0;

    protected virtual void MoveTowardPosition()
    {

    }

    protected virtual void RotateTowardPosition()
    {

    }

    protected virtual void Die()
    {

    }

    public virtual void TakeDamage(int damage, AmmunitionEffect effect = AmmunitionEffect.None)
    {
        if (!Alive) return;

        Health -= damage;
        if (Health <= 0)
        {
        }
    }

    public void OnShot(Vector3 dir, AmmunitionData data)
    {
        TakeDamage(data.Damage);
    }
}
