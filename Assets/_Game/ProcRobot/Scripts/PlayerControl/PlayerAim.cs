using System;
using System.Collections;
using UnityEngine;

public class PlayerAim : ShooterBase
{
    [field: SerializeField] public WeaponBase CurrentWeapon { get; private set; } = default;
    
    private PlayerHandler _player;

    [SerializeField] private Transform _headTransform;


    private bool _aim = false;

    private Camera _cam;

    public Vector3 Direction { get; private set; }

    public override Action<RaycastHit> OnShoot { get; set; }

    private Quaternion _startArmRotation;
    private Quaternion _startHeadRotation;

    private void Start()
    {
        _cam = Camera.main;

        _startHeadRotation = _headTransform.localRotation;

        _player = GetComponent<PlayerHandler>();
        _player.OnAimDown += AimIn;
        _player.OnAimUp += AimOut;


        if (CurrentWeapon) CurrentWeapon.OnEquipped(this);
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
                Direction = (hit.point - CurrentWeapon.WeaponTransform.position).normalized;

                CurrentWeapon.WeaponTransform.forward = Direction;
                _headTransform.forward = Direction;
            }

            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
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


        if (Physics.Raycast(ray, out mouseHit, Mathf.Infinity , layerMask: DetectionLayer))
        {
            return true;
        }

        return false;
    }

    private void ShootRayFromArm()
    {
        if (Physics.Raycast(CurrentWeapon.WeaponTransform.position, Direction, out RaycastHit hit, Mathf.Infinity, layerMask: DetectionLayer))
        {
            OnShoot?.Invoke(hit);
        }    
    }

    private void AimOut()
    {
        _aim = false;
        CurrentWeapon.WeaponTransform.localRotation = _startArmRotation;
        _headTransform.localRotation = _startHeadRotation;
    }

}
