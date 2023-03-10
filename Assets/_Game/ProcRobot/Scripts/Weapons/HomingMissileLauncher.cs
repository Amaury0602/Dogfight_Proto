using UnityEngine;

public class HomingMissileLauncher : ProjectileWeapon
{
    protected override void ShootHoming(Transform target)
    {
        base.ShootHoming(target);

        ProjectileBase proj = Instantiate(_projectile, Cannon.position, Quaternion.LookRotation(WeaponTransform.forward));

        proj.Initialize(_shooterLayer, Data);

        proj.OnWeaponFire(target);
    }
}
