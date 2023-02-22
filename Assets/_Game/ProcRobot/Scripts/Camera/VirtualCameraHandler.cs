using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraHandler : MonoBehaviour
{
    private CinemachineBrain _brain;

    private CinemachineVirtualCamera _currentCam;
    void Start()
    {
        _brain = GetComponent<CinemachineBrain>();
        _currentCam = (CinemachineVirtualCamera)_brain.ActiveVirtualCamera;
    }
}
