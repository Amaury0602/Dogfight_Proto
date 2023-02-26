using System.Collections;
using UnityEngine;
public class SecondaryLaser : SecondaryWeaponBase
{

    [SerializeField] private ParticleSystem _chargeBeamFX;
    [SerializeField] private Transform _debugBeam;
    [SerializeField] private float _timeToFullCharge;

    private Coroutine _beamRoutine = null;
    public override void OnStart()
    {
        _chargeBeamFX.Play();

        _beamRoutine = StartCoroutine(ChargeBeam());
    }

    private IEnumerator ChargeBeam()
    {
        float power = 0f;
        float elapsed = 0f;
        while (IsActive)
        {
            elapsed += Time.deltaTime;
            power = Mathf.Clamp01(elapsed / _timeToFullCharge);

            yield return null;
        }
    }

    private void ReleaseBeam(float power)
    {

    }
    public override void OnExit()
    {
        //if (_beamRoutine != null) StopCoroutine(_beamRoutine); 
        _chargeBeamFX.Stop();
    }

}
