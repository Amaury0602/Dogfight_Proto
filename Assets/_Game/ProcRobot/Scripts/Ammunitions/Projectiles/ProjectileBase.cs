using System.Collections;
using UnityEngine;

public class ProjectileBase : AmmunitionBase
{
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _effectRadius;
    [SerializeField] protected int _maxColliders;
    [SerializeField] protected bool _splashDamage;

    protected Collider[] _hitColliders = default;

    protected Vector3 _lastPos = default;
    protected float _timer;

    protected Coroutine _flyRoutine = null;

    [SerializeField] protected LayerMask _mask;
    protected AmmunitionData _data;


    private IShootableEntity _directShotEntity = null;

    private void Awake()
    {
        if (_hitColliders == null) _hitColliders = new Collider[_maxColliders];
    }

    public void Initialize(LayerMask mask, AmmunitionData data)
    {
        _directShotEntity = null;
        _mask = mask;
        _data = data;
        _data.Type = AmmunitionType.Projectile;
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

    public virtual void OnWeaponFire(Transform target) { }

    protected virtual IEnumerator FlyTowardsTarget(Vector3 target)
    {
        _timer = 4f;
        _lastPos = transform.position;
        Vector3 dir = target - transform.position;
        while (_timer >= 0)
        {
            _lastPos = transform.position;
            transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
            if (Physics.Linecast(_lastPos, transform.position, out RaycastHit hit, _mask))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("OBSTACLE"))
                {
                    FXHandler.Instance.PlayDebris(hit.point + hit.normal, Quaternion.LookRotation(hit.normal), _data.Damage);
                }
                else
                {
                    IShootableEntity direct = hit.collider.GetComponent<IShootableEntity>();
                    if (direct != null)
                    {
                        _directShotEntity = direct;
                        _directShotEntity.OnShot((hit.collider.transform.position - transform.position).normalized, _data);
                    }
                }

                
                ReachedTarget();
                yield break;
            }
            _timer -= Time.deltaTime;
            yield return null;
        }

        ReachedTarget();
    }

    protected virtual void ReachedTarget()
    {
        if (_flyRoutine != null) StopCoroutine(_flyRoutine);
        if (_splashDamage) DamageSurroundings();
        Destroy(gameObject);
    }

    private void DamageSurroundings()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, _effectRadius, _hitColliders);
        for (int i = 0; i < numColliders; i++)
        {
            IShootableEntity shootable = _hitColliders[i].GetComponent<IShootableEntity>();
            if (shootable != null)
            {
                if (shootable == _directShotEntity) continue;

                Vector3 dir = _hitColliders[i].transform.position - transform.position;
                shootable.OnShot(dir.normalized, _data);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_lastPos, 2f * Vector3.one);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ((_mask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("OBSTACLE"))
            {
                FXHandler.Instance.PlayDebris(transform.position - transform.forward, Quaternion.LookRotation(-transform.forward), _data.Damage);
            }

            IShootableEntity directShot = other.GetComponent<IShootableEntity>();
            if (directShot != null)
            {
                _directShotEntity = directShot;
                _directShotEntity.OnShot((other.transform.position - transform.position).normalized, _data);

            }
            ReachedTarget();
        }

    }
}
