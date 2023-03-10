using System.Collections;
using UnityEngine;

public class HomingProjectileBase : ProjectileBase
{
    [SerializeField] protected float _rotationSpeed;

    public override void OnWeaponFire(Transform target)
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 0.1f, _hitColliders, _mask);

        if (numColliders > 0)
        {
            ReachedTarget();
            return;
        }

        _flyRoutine = StartCoroutine(HomeTowardsTarget(target));

    }

    protected virtual IEnumerator HomeTowardsTarget(Transform target)
    {
        _timer = 4f;
        _lastPos = transform.position;
        Vector3 dir = target.position - transform.position;
        while (_timer >= 0)
        {
            dir = target.position - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.deltaTime);

            _lastPos = transform.position;
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
            if (Physics.Linecast(_lastPos, transform.position, out RaycastHit hit, _mask))
            {
                ReachedTarget();
                yield break;
            }
            _timer -= Time.deltaTime;
            yield return null;
        }

        ReachedTarget();
    }
}
