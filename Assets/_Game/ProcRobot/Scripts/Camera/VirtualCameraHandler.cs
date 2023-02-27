using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraHandler : MonoBehaviour
{
    private CinemachineBrain _brain;

    private CinemachineVirtualCamera _currentCam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    [SerializeField] private NoiseSettings _noiseSettings;

    private Coroutine _shakeRoutine = default;


    [Header("Shake Debug Variables")]
    [SerializeField] private float _testAmplitude;
    [SerializeField] private float _testFrequency;
    [SerializeField] private float _testDuration;

    void Start()
    {
        _brain = GetComponent<CinemachineBrain>();
        _currentCam = (CinemachineVirtualCamera)_brain.ActiveVirtualCamera;
        _perlin = _currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_NoiseProfile = _noiseSettings;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (_shakeRoutine != null)
            {
                StopCoroutine(_shakeRoutine);
            }

            _shakeRoutine = StartCoroutine(Shake(_testAmplitude, _testFrequency, _testDuration));
        }
    }
#endif


    public IEnumerator Shake(float amplitude, float frequency, float duration)
    {
        float elapsed = 0f;

        _perlin.m_FrequencyGain = frequency;
        _perlin.m_AmplitudeGain = amplitude;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        _perlin.m_AmplitudeGain = 0f;
        _perlin.m_FrequencyGain = 0f;
    }
} 
