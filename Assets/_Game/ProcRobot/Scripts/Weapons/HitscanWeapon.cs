using UnityEngine;

public class HitscanWeapon : WeaponBase
{
    [SerializeField] private AmmunitionData _bulletData;
    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);

        SpawnBulletTrail(hit.point);

        IShootable shootable = hit.collider.GetComponent<IShootable>();
        if (shootable != null)
        {
            Vector3 dir = hit.point - WeaponTransform.position;
            shootable.OnShot(dir.normalized, _bulletData);
        }
    }

    protected override void PlayerShootInDirection(Vector3 dir)
    {
        base.PlayerShootInDirection(dir);
        SpawnBulletTrail(WeaponTransform.position + dir * 50f);
    }


    protected virtual void SpawnBulletTrail(Vector3 targetPoint)
    {
        FXHandler.Instance.PlayBulletTrail(Cannon.position, targetPoint + Random.insideUnitSphere * 2f);
    }
}
