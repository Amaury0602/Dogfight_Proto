using System;
using System.Collections;
using UnityEngine;

public class PlayerAim : ShooterBase
{
    [field: SerializeField] public WeaponBase CurrentWeapon { get; private set; } = default;
    
    private PlayerHandler _player;

    [SerializeField] private Transform _headTransform;
    [SerializeField] private AimVisuals _visuals;

    [SerializeField] private LayerMask _floorLayer;


    private bool _aim = false;

    private Camera _cam;

    public Vector3 Direction { get; private set; }

    public override Action<RaycastHit> OnShoot { get; set; }

    private Quaternion _startArmRotation;
    private Quaternion _startHeadRotation;

    [SerializeField] private Transform _debug;

    private void Start()
    {
        Cursor.visible = false;
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

        _visuals.EnableLine();

        StartCoroutine(Aim());
    }

    private IEnumerator Aim()
    {
        

        while (_aim)
        {

            Vector3 start = CurrentWeapon.Cannon.position;
            Vector3 end;

            if (ShootRayToFloorPosition(out RaycastHit hit))
            {
                Direction = (hit.point - CurrentWeapon.WeaponTransform.position).normalized;
                Direction = new Vector3(Direction.x, 0, Direction.z);
                end = hit.point;
                CurrentWeapon.WeaponTransform.forward = Direction;
                _headTransform.forward = Direction;
            }

            end = start + CurrentWeapon.WeaponTransform.forward * 75f;
            _visuals.UpdateLine(start, end);

            Vector3 worldPos = hit.point;
            worldPos.y = CurrentWeapon.WeaponTransform.position.y;


            if (Physics.Raycast(CurrentWeapon.WeaponTransform.position, Direction, out RaycastHit hit2, Mathf.Infinity, layerMask: DetectionLayer))
            {
                _debug.position = hit2.point;
                worldPos = hit2.point;
            }

            PlayerUICursor.Instance.UpdatePosition(_cam.WorldToScreenPoint(worldPos));


            //SHOOTING
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
            {
                ShootRayFromArm();
            }
            yield return null;
        }
    }

    private bool ShootRayToFloorPosition(out RaycastHit mouseHit)
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        mouseHit = default;

        if (Physics.Raycast(ray, out mouseHit, Mathf.Infinity, layerMask: _floorLayer))
        {
            return true;
        }

        return false;
    }

    //private bool ShootRayToMousePosition(out RaycastHit mouseHit)
    //{
    //    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
    //    mouseHit = default;

    //    if (Physics.Raycast(ray, out mouseHit, Mathf.Infinity , layerMask: DetectionLayer))
    //    {
    //        return true;
    //    }

    //    return false;
    //}

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
        _visuals.DisableLine();
        CurrentWeapon.WeaponTransform.localRotation = _startArmRotation;
        _headTransform.localRotation = _startHeadRotation;
    }

}
