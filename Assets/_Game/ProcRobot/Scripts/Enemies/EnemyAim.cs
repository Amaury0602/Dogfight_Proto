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

    public void SetWeapon()
    {
        CurrentWeapon.OnEquipped(this);
    }

    public void AimAt(Vector3 position)
    {
        position.y = transform.position.y;   
        Direction = (position - CurrentWeapon.WeaponTransform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(Direction);

        CurrentWeapon.WeaponTransform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _aimingSpeed * Time.deltaTime);

        //CurrentWeapon.WeaponTransform.forward = Direction;
        _headTransform.forward = Direction;


        if (Physics.SphereCast(CurrentWeapon.transform.position, _aimRadius, CurrentWeapon.transform.forward, out RaycastHit hit, maxDistance: 50f, layerMask: _playerLayer))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("PLAYER"))   OnShoot?.Invoke(hit);
        }
    }

    public void StopAiming()
    {

    }
}
