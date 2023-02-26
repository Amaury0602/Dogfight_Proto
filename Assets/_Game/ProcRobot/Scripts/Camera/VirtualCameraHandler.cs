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

    void Start()
    {
        _brain = GetComponent<CinemachineBrain>();
        _currentCam = (CinemachineVirtualCamera)_brain.ActiveVirtualCamera;
        _perlin = _currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_NoiseProfile = _noiseSettings;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //_perlin.
        }
    }

    //private void StopShake()
    //{
    //    _noiseSettings.ampl
    //}
}
