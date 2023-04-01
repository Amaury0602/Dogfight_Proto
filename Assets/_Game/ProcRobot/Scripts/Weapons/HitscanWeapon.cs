using UnityEngine;

public class HitscanWeapon : WeaponBase
{
    protected override void Shoot(RaycastHit hit)
    {
        base.Shoot(hit);

        SpawnBulletTrail(hit.point);

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("OBSTACLE"))
        {
            FXHandler.Instance.PlayDebris(hit.point + hit.normal, Quaternion.LookRotation(hit.normal), Data.Damage);
        }
        else
        {
            IShootableEntity shootable = hit.collider.GetComponent<IShootableEntity>();
            if (shootable != null)
            {
                Vector3 dir = hit.point - WeaponTransform.position;
                shootable.OnShot(dir.normalized, Data);
            }
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
