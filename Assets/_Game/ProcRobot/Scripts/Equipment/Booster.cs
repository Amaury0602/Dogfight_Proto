using UnityEngine;
using System;
using System.Collections;

public class Booster : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _baseTrail;
    [SerializeField] private ParticleSystem[] _boostTrail;
    [SerializeField] private ParticleSystem _burstFX;
    [SerializeField] private float _speedBoost;
    [SerializeField] private float _speedImpulse;

    [Header("Energy Management")]
    [SerializeField] private float _maxEnergy;
    [SerializeField] private float _energyConsumeSpeed;
    [SerializeField] private float _energyRegainSpeed;
    [SerializeField] private float _energyConsumedByDash;
    private float _actualEnergy;

    public float EnergyRatio => _actualEnergy / _maxEnergy;

    [SerializeField, Range(0,1)] private float _dashThreshold;
    public float DashThreshold => _dashThreshold;


    public Action<float, float> OnBoost = default;
    public Action<float> BoostConsumed = default;
    public Action EnergyFullyConsumed = default;

    private bool _active = false;


    [Header("Screen effects")]
    [SerializeField] private float _shakePower;

    private Coroutine _energyFillRoutine = default;

    private void Start()
    {
        _actualEnergy = _maxEnergy;

        PlayerInputs.Instance.OnSpaceDown += Boost;
        Player.Instance.Handler.OnMovementStopped += OnStopped;
    }

    private void Boost()
    {
        //if (EnergyRatio < _dashThreshold) return;
        _active = true;

        if (_energyFillRoutine != null) StopCoroutine(_energyFillRoutine);
        _energyFillRoutine = StartCoroutine(ConsumeBoost());


        OnBoost?.Invoke(_speedImpulse, _speedBoost);

        for (int i = 0; i < _baseTrail.Length; i++)
        {
            _baseTrail[i].Stop();
        }
        for (int i = 0; i < _boostTrail.Length; i++)
        {
            _boostTrail[i].Play();
        }

        _burstFX.Play();

        VirtualCameraHandler.Instance.Shake(_shakePower, _shakePower, 0.1f);
    }

    private IEnumerator ConsumeBoost()
    {
        while(_actualEnergy >= 0)
        {

            print(" CONSUMIIIIIIIING ");

            _actualEnergy -= _energyConsumeSpeed * Time.deltaTime;
            BoostConsumed?.Invoke(EnergyRatio);
            yield return null;
        }

        EnergyFullyConsumed?.Invoke();

        _actualEnergy = 0;
        OnStopped();
    }
    
    private IEnumerator RegainBoost()
    {
        while(!_active && _actualEnergy < _maxEnergy)
        {
            print("Regain boost" + _actualEnergy);

            _actualEnergy += _energyRegainSpeed * Time.deltaTime;
            BoostConsumed?.Invoke(EnergyRatio);
            yield return null;
        }

        _actualEnergy = _maxEnergy;
    }

    private void OnStopped()
    {

        print("ON STOPPED BOOST");

        //if (!_active) return;

        //_active = false;

        if (_energyFillRoutine != null) StopCoroutine(_energyFillRoutine);
        _energyFillRoutine = StartCoroutine(RegainBoost());

        for (int i = 0; i < _baseTrail.Length; i++)
        {
            _baseTrail[i].Play();
        }
        for (int i = 0; i < _boostTrail.Length; i++)
        {
            _boostTrail[i].Stop();
        }
    }
}
