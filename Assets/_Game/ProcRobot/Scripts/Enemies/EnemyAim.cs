using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : ShooterBase
{

    [SerializeField] private Transform _headTransform;

    public Vector3 Direction { get; private set; }
    [field: SerializeField] public WeaponBase CurrentWeapon { get; private set; } = default;
    public override Action<RaycastHit> OnShoot { get; set; }

    [SerializeField] private float _aimingSpeed;
    [SerializeField] private float _aimRadius;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _sightAngle;
    [SerializeField] private float _sightDistance;

    public Action OnGainSight = default;
    public Action<Vector3> OnLostSight = default; // Vector3 is last known position
    private bool _targetInSight = false;

    private Vector3 _lastKnownPosition = default;

    private RaycastHit[] _visionHits;

    private void Awake()
    {
        _visionHits = new RaycastHit[5];
    }

    public void SetWeapon()
    {
        CurrentWeapon.OnEquipped(this);
    }

    public void AimAt(Vector3 position)
    {
        if ((position - transform.position).sqrMagnitude > _sightDistance * _sightDistance) return; 

        position.y = transform.position.y;   
        Direction = (position - CurrentWeapon.WeaponTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(Direction);
        CurrentWeapon.WeaponTransform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _aimingSpeed * Time.deltaTime);
        _headTransform.forward = Direction;

        int visibleObjects = Physics.SphereCastNonAlloc(transform.position, 5f, transform.forward, _visionHits, Mathf.Infinity, layerMask: _playerLayer);


        bool obstacle = false;
        
        Vector3 dir = (position - transform.position).normalized;
        float dProduct = Vector3.Dot(dir, transform.forward);
        
        bool inSight = dProduct >= Mathf.Cos(_sightAngle);


#if UNITY_EDITOR
        Debug.DrawRay(transform.position, transform.forward * _sightDistance, Color.blue);
#endif

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit h, maxDistance: _sightDistance, layerMask: _playerLayer))
        {
            if (h.collider.gameObject.layer != LayerMask.NameToLayer("PLAYER"))
            {
                if ((h.point - transform.position).sqrMagnitude < (position - transform.position).sqrMagnitude)
                {
                    obstacle = true;
                }
            }
        }

        if (!obstacle && inSight) // IN SIGHT
        {
            _lastKnownPosition = position;
            if (!_targetInSight)
            {
                _targetInSight = true;
                OnGainSight?.Invoke();
            }
        }
        else
        {
            if (_targetInSight)
            {
                _targetInSight = false;
                OnLostSight?.Invoke(_lastKnownPosition);
            }
        }

        //shoot cast
        if (Physics.SphereCast(CurrentWeapon.transform.position, _aimRadius, CurrentWeapon.transform.forward, out RaycastHit hit, maxDistance: 50f, layerMask: _playerLayer))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
            {
                OnShoot?.Invoke(hit);
            }
        }
    }

    public void StopAiming()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_lastKnownPosition, ((Mathf.Sin(Time.time * 5f) + 1) / 2) + 0.5f * 3f);
    }
}
