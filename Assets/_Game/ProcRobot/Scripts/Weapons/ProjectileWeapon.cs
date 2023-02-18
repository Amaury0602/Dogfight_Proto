using UnityEngine;

public class ProjectileWeapon : WeaponBase
{
    [SerializeField] protected ProjectileBase _projectile;

    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);

        ProjectileBase proj = Instantiate(_projectile, _cannon.position, Quaternion.LookRotation(WeaponTransform.forward));
        proj.OnWeaponFire(hit.point);
    }
}
