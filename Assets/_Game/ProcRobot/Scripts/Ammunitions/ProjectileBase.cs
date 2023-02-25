using System.Collections;
using UnityEngine;

public class ProjectileBase : AmmunitionBase
{
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _effectRadius;
    [SerializeField] protected int _maxColliders;

    protected Collider[] _hitColliders = default;

    private Vector3 _lastPos = default;
    private float _timer;

    private Coroutine _flyRoutine = null;

    [SerializeField] private LayerMask _mask;

    private void Awake()
    {
        if (_hitColliders == null) _hitColliders = new Collider[_maxColliders];
        Data.Type = AmmunitionType.Projectile;
    }
    public void SetLayer(LayerMask mask)
    {
        _mask = mask;
    }

    public virtual void OnWeaponFire(Vector3 target)
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 0.1f, _hitColliders, _mask);

        if (numColliders > 0) 
        {
            ReachedTarget();
            return;
        } 
        _flyRoutine = StartCoroutine(FlyTowardsTarget(target));
    }

    protected virtual IEnumerator FlyTowardsTarget(Vector3 target)
    {
        _timer = 2f;
        _lastPos = transform.position;
        Vector3 dir = target - transform.position;
        while (_timer >= 0)
        {
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;

            if (Physics.Linecast(_lastPos, transform.position, out RaycastHit hit, _mask))
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
        if (_flyRoutine != null) StopCoroutine(_flyRoutine);
        DamageSurroundings();
        Destroy(gameObject);
    }

    private void DamageSurroundings()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _effectRadius, _hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            IShootable shootable = _hitColliders[i].GetComponent<IShootable>();
            if (shootable != null)
            {
                Vector3 dir = _hitColliders[i].transform.position - transform.position;
                shootable.OnShot(dir.normalized, Data);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _effectRadius);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((_mask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            ReachedTarget();
        }

    }
}
