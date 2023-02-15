using System;
using System.Collections;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private PlayerHandler _player;

    [SerializeField] private Transform _aimingArm;
    [SerializeField] private Transform _headTransform;

    private bool _aim = false;

    private Camera _cam;

    public Vector3 Direction { get; private set; }


    [SerializeField] private LayerMask _aimDetectionLayer;

    [SerializeField] private Transform _debugCube;

    public Action OnShoot = default;

    private Quaternion _startArmRotation;
    private Quaternion _startHeadRotation;

    private void Start()
    {
        _cam = Camera.main;

        _startArmRotation = _aimingArm.localRotation;
        _startHeadRotation = _headTransform.localRotation;

        _player = GetComponent<PlayerHandler>();
        _player.OnAimDown += AimIn;
        _player.OnAimUp += AimOut;
    }
    private void AimIn()
    {
        _aim = true;
        StartCoroutine(Aim());
    }


    private IEnumerator Aim()
    {
        while(_aim)
        {


            if (ShootRayToMousePosition(out RaycastHit hit))
            {
                Direction = (hit.point - _aimingArm.position).normalized;

                _aimingArm.forward = Direction;
                _headTransform.forward = Direction;
            }

            if (Input.GetMouseButton(0))
            {
                ShootRayFromArm();
            }
            yield return null;
        }

    }


    private bool ShootRayToMousePosition(out RaycastHit mouseHit)
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        mouseHit = default;


        if (Physics.Raycast(ray, out mouseHit, Mathf.Infinity , layerMask: _aimDetectionLayer))
        {
            return true;
        }

        return false;
    }

    private void ShootRayFromArm()
    {
        OnShoot?.Invoke();
        if (Physics.Raycast(_aimingArm.position, Direction, out RaycastHit hit, Mathf.Infinity, layerMask: _aimDetectionLayer))
        {
            _debugCube.position = hit.point;
        }    
    }

    private void AimOut()
    {
        _aim = false;
        _aimingArm.localRotation = _startArmRotation;
        _headTransform.localRotation = _startHeadRotation;
    }

}
