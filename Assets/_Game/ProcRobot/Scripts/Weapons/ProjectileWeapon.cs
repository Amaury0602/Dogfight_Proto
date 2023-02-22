using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [SerializeField] protected ProjectileBase _projectile;
    [SerializeField] protected LayerMask _shooterLayer;

    public override void OnEquipped(ShooterBase shooter)
    {
        base.OnEquipped(shooter);
        _shooterLayer = shooter.DetectionLayer;
    }

    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);

        ProjectileBase proj = Instantiate(_projectile, Cannon.position, Quaternion.LookRotation(WeaponTransform.forward));

        proj.SetLayer(_shooterLayer);
        
        proj.OnWeaponFire(hit.point);
    }

    protected override void PlayerShootInDirection(Vector3 dir)
    {
        base.PlayerShootInDirection(dir);

        ProjectileBase proj = Instantiate(_projectile, Cannon.position, Quaternion.LookRotation(dir));

        proj.SetLayer(_shooterLayer);

        proj.OnWeaponFire(proj.transform.position + dir);
    }
}
