using System.Collections;
using UnityEngine;

public class ProjectileBase : AmmunitionBase
{
    [SerializeField] protected float _moveSpeed;

    private void Awake()
    {
        Data.Type = AmmunitionType.Projectile;
    }

    public virtual void OnWeaponFire(Vector3 target)
    {
        StartCoroutine(FlyTowardsTarget(target));
    }

    protected virtual IEnumerator FlyTowardsTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        while (dir.sqrMagnitude > 1f)
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;

            dir = target - transform.position;

            yield return null;
        }

        ReachedTarget();
    }

    protected virtual void ReachedTarget()
    {
        Destroy(gameObject);
    }
}
