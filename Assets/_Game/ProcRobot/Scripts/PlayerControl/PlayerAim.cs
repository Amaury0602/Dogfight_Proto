using System;
using UnityEngine;

public class PlayerAim : ShooterBase
{
    [field: SerializeField] public WeaponBase CurrentWeapon { get; private set; } = default;
    [field: SerializeField] public SecondaryWeaponBase SecondaryWeapon { get; private set; } = default;
    
    [SerializeField] private Transform _headTransform;
    [SerializeField] private AimVisuals _visuals;
    public Vector3 Direction { get; private set; }

    public override Action<RaycastHit> OnShoot { get; set; }
    public override Action<Vector3> OnShootInDirection { get; set; }

    private Player _player;


    [SerializeField] private float _weaponRotSpeed;

    private void Start()
    {
        _player = GetComponent<Player>();
        _player.OnDeath += OnDeath;
        
        PlayerUICursor.Instance.OnProjectedPoint += FollowCursor;

        PlayerInputs.Instance.OnRightMouseDown += OnRightMouseDown;
        PlayerInputs.Instance.OnRightMouseHold += OnRightMouseHold;
        PlayerInputs.Instance.OnRightMouseUp += OnRightMouseUp;

        if (CurrentWeapon) CurrentWeapon.OnEquipped(this);
    }

    

    private void FollowCursor(Vector3 aimPoint)
    {
        Vector3 start = CurrentWeapon.Cannon.position;
        Vector3 end = start + CurrentWeapon.WeaponTransform.forward * 75f; ;

        Direction = (aimPoint - CurrentWeapon.WeaponTransform.position).normalized;

        Quaternion dir = Quaternion.LookRotation(Direction);
        CurrentWeapon.transform.rotation = Quaternion.Slerp(CurrentWeapon.transform.rotation, dir, _weaponRotSpeed * Time.deltaTime);

        _headTransform.forward = Direction;

        if (Physics.Raycast(CurrentWeapon.WeaponTransform.position, Direction, out RaycastHit hit, Mathf.Infinity, layerMask: DetectionLayer))
        {
            if ((transform.position - aimPoint).sqrMagnitude > (transform.position - hit.point).sqrMagnitude)
            {
                end = hit.point;
            }
        }

        _visuals.UpdateLine(start, end);


        //SHOOTING
        if (Input.GetMouseButton(0))
        {
            ShootRayFromArm();
        }
    }

    private void OnRightMouseDown()
    {
        CurrentWeapon.OnUnequipped(this);
        SecondaryWeapon.OnEquipped(this);
    }

    private void OnRightMouseHold()
    {

    }

    private void OnRightMouseUp()
    {
        CurrentWeapon.OnEquipped(this);
        SecondaryWeapon.OnUnequipped(this);
    }

    private void ShootRayFromArm()
    {
        if (Physics.Raycast(CurrentWeapon.WeaponTransform.position, Direction, out RaycastHit hit, Mathf.Infinity, layerMask: DetectionLayer))
        {
            OnShoot?.Invoke(hit);
        }
        else
        {
            OnShootInDirection?.Invoke(CurrentWeapon.WeaponTransform.forward);
        }    
    }

    private void OnDeath()
    {
        _player.OnDeath -= OnDeath;
        PlayerUICursor.Instance.OnProjectedPoint -= FollowCursor;
    }

}
