using System;
using UnityEngine;

public class PlayerAim : ShooterBase
{
    [field: SerializeField] public WeaponBase CurrentWeapon { get; private set; } = default;
    
    [SerializeField] private Transform _headTransform;
    [SerializeField] private AimVisuals _visuals;
    public Vector3 Direction { get; private set; }

    public override Action<RaycastHit> OnShoot { get; set; }
    public override Action<Vector3> OnShootInDirection { get; set; }

    private Player _player;

    private void Start()
    {
        Cursor.visible = false;

        _player = GetComponent<Player>();
        _player.OnDeath += OnDeath;
        
        PlayerUICursor.Instance.OnProjectedPoint += FollowCursor;

        if (CurrentWeapon) CurrentWeapon.OnEquipped(this);
    }
    private void FollowCursor(Vector3 aimPoint)
    {
        Vector3 start = CurrentWeapon.Cannon.position;
        Vector3 end = start +CurrentWeapon.WeaponTransform.forward * 75f; ;

        Direction = (aimPoint - CurrentWeapon.WeaponTransform.position).normalized;
        CurrentWeapon.WeaponTransform.forward = Direction;
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
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            ShootRayFromArm();
        }
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
