using UnityEngine;

public class HitscanWeapon : WeaponBase
{
    [SerializeField] private AmmunitionData _bulletData;
    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);

        IShootable shootable = hit.collider.GetComponent<IShootable>();
        if (shootable != null)
        {
            Vector3 dir = hit.point - WeaponTransform.position;
            shootable.OnShot(dir.normalized, _bulletData);
        }
    }
}
