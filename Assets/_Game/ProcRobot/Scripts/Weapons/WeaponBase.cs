using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public Transform WeaponTransform { get; private set; } = default;

    [SerializeField] public Transform Cannon;

    [SerializeField] protected ParticleSystem _muzzleFlash;

    [SerializeField] protected float _fireCD;
    [SerializeField] protected float _recoil;
    [SerializeField] protected int _muzzleFlashParticleCount;

    [field: SerializeField] public AmmunitionData Data { get; private set; }

    protected bool _canShoot;
    private float _currentCD;

    private Vector3 _startLocalPosition;

    public Action OnShotFired = default;

    protected virtual void Awake()
    {
        _startLocalPosition = WeaponTransform.localPosition;
    }

    public virtual void OnEquipped(ShooterBase shooter)
    {
        _canShoot = true;
        shooter.OnShoot += TryShoot;
        shooter.OnShotHoming += OnShotHoming;
        shooter.OnShootInDirection += TryShootInDirection;

        _currentCD = _fireCD;
    }

    private void OnShotHoming(Transform obj)
    {
        throw new NotImplementedException();
    }

    public virtual void TryShootInDirection(Vector3 dir)
    {
        if (_canShoot)
        {
            PlayerShootInDirection(dir);
            StartCoroutine(ResetFireCoolDown());
        }
    }

    public virtual void OnUnequipped(ShooterBase shooter)
    {
        _canShoot = false;
        shooter.OnShoot -= TryShoot;
        shooter.OnShootInDirection -= TryShootInDirection;
    }

    private void TryShoot(RaycastHit hit)
    {
        if (_canShoot)
        {
            Shoot(hit);
            StartCoroutine(ResetFireCoolDown());
        }
    }

    protected virtual void Shoot(RaycastHit hit)
    {
        OnShotFired?.Invoke(); // only for feedback reasons

        if (_muzzleFlash) _muzzleFlash.Emit(_muzzleFlashParticleCount);

        WeaponTransform.DOKill();
        WeaponTransform.localPosition = _startLocalPosition;
        WeaponTransform
            .DOLocalMoveZ(WeaponTransform.localPosition.z - _recoil, 0.05f)
            .SetLoops(2, LoopType.Yoyo);
    }

    protected virtual void PlayerShootInDirection(Vector3 dir)
    {
        OnShotFired?.Invoke(); // only for feedback reasons

        if (_muzzleFlash) _muzzleFlash.Emit(_muzzleFlashParticleCount);

        WeaponTransform.DOKill();

        WeaponTransform.localPosition = _startLocalPosition;
        WeaponTransform
            .DOLocalMoveZ(WeaponTransform.localPosition.z - _recoil, 0.05f)
            .SetLoops(2, LoopType.Yoyo);
    }

    protected virtual IEnumerator ResetFireCoolDown()
    {
        _canShoot = false;
        while (_currentCD >= 0)
        {
            _currentCD -= Time.deltaTime;
            yield return null;
        }

        _currentCD = _fireCD;
        _canShoot = true;
    }
}
