using UnityEngine;
using System;

public class Player : MonoBehaviour, IShootable
{
    [field: SerializeField] public int Health { get; private set; }

    public PlayerHandler Handler { get; private set; }


    public static Player Instance;

    public bool Alive => Health > 0;

    public Action<float> OnTakeDamage = default;
    public Action OnDeath = default;

    private void Awake()
    {
        Handler = GetComponent<PlayerHandler>();
        Instance = this;
    }

    public void OnShot(Vector3 dir, AmmunitionData data)
    {
        if (!Alive) return;

        VirtualCameraHandler.Instance.Shake(0.25f, 0.25f, 0.1f);

        OnTakeDamage?.Invoke(data.Damage);

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

    public void OnShot(Vector3 dir, int damage, AmmunitionData data)
    {
        //throw new NotImplementedException();
    }
}
