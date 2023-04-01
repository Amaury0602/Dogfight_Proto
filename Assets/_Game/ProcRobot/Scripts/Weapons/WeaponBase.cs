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

    protected ShooterBase _shooter;

    [field: SerializeField] public AmmunitionData Data { get; private set; }

    protected bool _canShoot;
    private float _currentCD;

    private Vector3 _startLocalPosition;

    public Action OnShotFired = default;

    [SerializeField] private WeaponShake _shake;

    [Serializable]
    public struct WeaponShake
    {
        public float Amplitude;
        public float Frequency;
        public float Duration;
    }

    protected virtual void Awake()
    {
        _startLocalPosition = WeaponTransform.localPosition;
    }

    public virtual void OnEquipped(ShooterBase shooter)
    {
        _shooter = shooter;
        _canShoot = true;
        shooter.OnShoot += TryShoot;
        shooter.OnShotHoming += TryShootHoming;
        shooter.OnShootInDirection += TryShootInDirection;

        _currentCD = _fireCD;
    }

    

    public virtual void TryShootInDirection(Vector3 dir)
    {
        if (_canShoot)
        {
            ShakeCamera();
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
            ShakeCamera();
            Shoot(hit);
            StartCoroutine(ResetFireCoolDown());
        }
    }
    
    private void TryShootHoming(Transform target)
    {
        if (_canShoot)
        {
            ShakeCamera();
            ShootHoming(target);
            StartCoroutine(ResetFireCoolDown());
        }
    }

    protected virtual void ShootHoming(Transform target)
    {
        OnShotFired?.Invoke(); // only for feedback reasons

        if (_muzzleFlash) _muzzleFlash.Emit(_muzzleFlashParticleCount);

        WeaponTransform.DOKill();
        WeaponTransform.localPosition = _startLocalPosition;
        WeaponTransform
            .DOLocalMoveZ(WeaponTransform.localPosition.z - _recoil, 0.05f)
            .SetLoops(2, LoopType.Yoyo);
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

    private void ShakeCamera()
    {
        if (_shooter is PlayerAim)
        {
            VirtualCameraHandler.Instance.Shake(_shake.Amplitude, _shake.Frequency, _shake.Duration);
        }
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
