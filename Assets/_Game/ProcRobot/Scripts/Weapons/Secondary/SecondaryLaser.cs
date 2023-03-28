using System.Collections;
using UnityEngine;
public class SecondaryLaser : SecondaryWeaponBase
{

    [SerializeField] private ParticleSystem _chargeBeamFX;
    [SerializeField] private Transform _debugBeam;
    [SerializeField] private float _timeToFullCharge;

    private Coroutine _beamRoutine = null;

    private bool _isActive = false;

    [Header("Laser Visuals")]
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Vector2 _minMaxWidth;
    [SerializeField] private Vector2 _minMaxDuration;
    [SerializeField] private Vector2 _minMaxShake;

    private float _currentPower = 0;

    protected override void Awake()
    {
        base.Awake();
        _line.positionCount = 0;
    }


    public override void OnStart()
    {
        _isActive = true;
        _currentPower = 0;
        _beamRoutine = StartCoroutine(ChargeBeam());
        _line.positionCount = 2;
    }

    private IEnumerator ChargeBeam()
    {
        _chargeBeamFX.Play();
        float elapsed = 0f;
        _line.positionCount = 0;
        _currentPower = 0f;

        while (IsActive)
        {
            elapsed += Time.deltaTime;
            _currentPower = Mathf.Clamp01(elapsed / _timeToFullCharge);
            yield return null;
        }
    }

    private IEnumerator ReleaseBeam(Vector3 endPoint)
    {
        StopCoroutine(_beamRoutine);


        VirtualCameraHandler.Instance.Shake(Mathf.Lerp(_minMaxShake.x, _minMaxShake.y, _currentPower), 0.1f, 0.25f);


        _chargeBeamFX.Stop();

        float duration = Mathf.Lerp(_minMaxDuration.x, _minMaxDuration.y, _currentPower);
        float width = Mathf.Lerp(_minMaxWidth.x, _minMaxWidth.y, _currentPower);

        float elapsed = 0;

        _line.positionCount = 2;

        _line.SetPosition(0, Cannon.position);
        _line.SetPosition(1, endPoint);

        while (elapsed < duration)
        {
            _line.widthMultiplier = Mathf.Lerp(0, width, 1 - Mathf.Clamp01(elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return null;

        _line.positionCount = 0;
    }

    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);
        StartCoroutine(ReleaseBeam(hit.point));

        IShootableEntity shootable = hit.collider.GetComponent<IShootableEntity>();
        if (shootable != null)
        {
            Vector3 dir = hit.point - WeaponTransform.position;
            shootable.OnShot(dir.normalized, Mathf.FloorToInt(Data.Damage * _currentPower), Data);
        }
    }

    protected override void PlayerShootInDirection(Vector3 dir)
    {
        base.PlayerShootInDirection(dir);
        StartCoroutine(ReleaseBeam(Cannon.position + dir * 50f));
    }

    protected override IEnumerator ResetFireCoolDown()
    {
        yield return base.ResetFireCoolDown();
        if (_isActive) _beamRoutine = StartCoroutine(ChargeBeam());
    }


    public override void OnExit()
    {
        _isActive = false;

        _chargeBeamFX.Stop();
        _currentPower = 0f;

        if (_beamRoutine != null) StopCoroutine(_beamRoutine);
    }

}
