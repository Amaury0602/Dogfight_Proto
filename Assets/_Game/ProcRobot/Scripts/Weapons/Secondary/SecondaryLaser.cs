using System.Collections;
using UnityEngine;
public class SecondaryLaser : SecondaryWeaponBase
{

    [SerializeField] private ParticleSystem _chargeBeamFX;
    [SerializeField] private Transform _debugBeam;
    [SerializeField] private float _timeToFullCharge;

    private Coroutine _beamRoutine = null;


    [Header("Laser Visuals")]
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Vector2 _minMaxWidth;
    [SerializeField] private Vector2 _minMaxDuration;

    private float _currentPower = 0;


    public override void OnStart()
    {
        _chargeBeamFX.Play();
        _currentPower = 0;
        _beamRoutine = StartCoroutine(ChargeBeam());

        _line.positionCount = 0;
    }

    private IEnumerator ChargeBeam()
    {
        float elapsed = 0f;
        while (IsActive)
        {
            elapsed += Time.deltaTime;
            _currentPower = Mathf.Clamp01(elapsed / _timeToFullCharge);
            yield return null;
        }
    }

    private IEnumerator ReleaseBeam(Vector3 endPoint)
    {


        float duration = Mathf.Lerp(_minMaxDuration.x, _minMaxDuration.y, _currentPower);
        float width = Mathf.Lerp(_minMaxWidth.x, _minMaxWidth.y, _currentPower);
#if UNITY_EDITOR
        print(_currentPower);
        print(duration);
        print(width);
#endif

        float elapsed = 0;

        _line.positionCount = 2;

        _line.SetPosition(0, Cannon.position);
        _line.SetPosition(1, endPoint);

        while (elapsed < duration)
        {
            _line.SetPosition(0, Cannon.position);
            elapsed += Time.deltaTime;
            _line.widthMultiplier = 1 - Mathf.Lerp(0, width, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }

        yield return null;

        _line.positionCount = 0;
    }

    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);
        StartCoroutine(ReleaseBeam(hit.point));

        IShootable shootable = hit.collider.GetComponent<IShootable>();
        if (shootable != null)
        {
            Vector3 dir = hit.point - WeaponTransform.position;
            shootable.OnShot(dir.normalized, Data.Damage * Mathf.FloorToInt(_currentPower), Data);
        }
    }

    protected override void PlayerShootInDirection(Vector3 dir)
    {
        base.PlayerShootInDirection(dir);
        StartCoroutine(ReleaseBeam(Cannon.position + dir * 50f));
    }


    public override void OnExit()
    {
        _chargeBeamFX.Stop();
    }

}
