using System.Collections;
using UnityEngine;

public class ProjectileBase : AmmunitionBase
{
    [SerializeField] protected float _moveSpeed;
    private Vector3 _lastPos = default;

    private float _timer;

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
        _timer = 5f;
        _lastPos = transform.position;
        Vector3 dir = target - transform.position;
        while (_timer >= 0)
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;

            if (Physics.Linecast(_lastPos, transform.position, out RaycastHit hit))
            {
                ReachedTarget();
                yield break;
            }

            _timer -= Time.deltaTime;

            _lastPos = transform.position;

            yield return null;
        }

        ReachedTarget();
    }

    protected virtual void ReachedTarget()
    {
        Destroy(gameObject);
    }
}
