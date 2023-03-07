using UnityEngine;
using System;

public class Booster : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _baseTrail;
    [SerializeField] private ParticleSystem[] _boostTrail;
    [SerializeField] private ParticleSystem _burstFX;
    [SerializeField] private float _speedBoost;
    [SerializeField] private float _speedImpulse;


    public Action<float, float> OnBoost = default;


    [SerializeField] private float _shakePower;

    private void Start()
    {
        PlayerInputs.Instance.OnSpaceDown += Boost;
        Player.Instance.Handler.OnMovementStopped += OnStopped;
    }

    private void Boost()
    {
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

    private void OnStopped()
    {
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
